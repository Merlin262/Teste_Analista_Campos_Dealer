using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TesteCamposDealer.Application.Handlers.Clientes.Queries.GetAllClientes;
using TesteCamposDealer.Controllers;
using TesteCamposDealer.Infrastructure.Data;
using TesteCamposDealer.Infrastructure.Repositories;

namespace TesteCamposDealer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var appAssembly = typeof(GetAllClientesHandler).Assembly;
            var mediator = BuildMediator(appAssembly);

            DependencyResolver.SetResolver(
                serviceType =>
                {
                    if (serviceType == typeof(ClienteController)) return new ClienteController(mediator);
                    if (serviceType == typeof(ProdutoController)) return new ProdutoController(mediator);
                    if (serviceType == typeof(VendaController)) return new VendaController(mediator);
                    if (serviceType == typeof(HomeController)) return new HomeController();
                    return null;
                },
                serviceType => Enumerable.Empty<object>()
            );
        }

        private static IMediator BuildMediator(Assembly appAssembly)
        {
            return new Mediator(serviceType =>
            {
                if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    var itemType = serviceType.GetGenericArguments()[0];
                    if (itemType.IsGenericType && itemType.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>))
                    {
                        var reqType = itemType.GetGenericArguments()[0];
                        var resType = itemType.GetGenericArguments()[1];

                        var valBehaviorType = appAssembly.GetType("TesteCamposDealer.Application.Behaviors.ValidationBehavior`2").MakeGenericType(reqType, resType);
                        var validatorType = typeof(FluentValidation.IValidator<>).MakeGenericType(reqType);

                        var validatorImplTypes = appAssembly.GetTypes()
                            .Where(t => !t.IsAbstract && !t.IsInterface && validatorType.IsAssignableFrom(t)).ToArray();

                        var validatorsArray = Array.CreateInstance(validatorType, validatorImplTypes.Length);
                        for (int i = 0; i < validatorImplTypes.Length; i++)
                        {
                            validatorsArray.SetValue(Activator.CreateInstance(validatorImplTypes[i]), i);
                        }

                        var validationBehavior = Activator.CreateInstance(valBehaviorType, new object[] { validatorsArray });
                        var behaviorArray = Array.CreateInstance(itemType, 1);
                        behaviorArray.SetValue(validationBehavior, 0);

                        return behaviorArray;
                    }

                    return Array.CreateInstance(itemType, 0);
                }

                var handlerType = appAssembly.GetTypes()
                    .FirstOrDefault(t => !t.IsAbstract && !t.IsInterface && serviceType.IsAssignableFrom(t));

                if (handlerType == null) return null;

                return Activator.CreateInstance(handlerType, new UnitOfWork(new AppDbContext()));
            });
        }
    }
}
