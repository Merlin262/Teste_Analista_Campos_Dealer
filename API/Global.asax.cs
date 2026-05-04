using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
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
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var appAssembly = typeof(GetAllClientesHandler).Assembly;
            var mediator = BuildMediator(appAssembly);

            GlobalConfiguration.Configuration.DependencyResolver = new MediatorDependencyResolver(mediator);
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

    internal class MediatorDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {
        private readonly IMediator _mediator;
        public MediatorDependencyResolver(IMediator mediator) { _mediator = mediator; }

        public System.Web.Http.Dependencies.IDependencyScope BeginScope() => this;

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(ClienteController)) return new ClienteController(_mediator);
            if (serviceType == typeof(ProdutoController)) return new ProdutoController(_mediator);
            if (serviceType == typeof(VendaController)) return new VendaController(_mediator);
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType) => Enumerable.Empty<object>();

        public void Dispose() { }
    }
}
