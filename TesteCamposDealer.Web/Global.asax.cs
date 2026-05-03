using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TesteCamposDealer.Web.Controllers;
using TesteCamposDealer.Web.Services;

namespace TesteCamposDealer.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            var api = new ApiClient(baseUrl);

            DependencyResolver.SetResolver(
                serviceType =>
                {
                    if (serviceType == typeof(ClienteController)) return new ClienteController(api);
                    if (serviceType == typeof(ProdutoController)) return new ProdutoController(api);
                    if (serviceType == typeof(VendaController)) return new VendaController(api);
                    if (serviceType == typeof(HomeController)) return new HomeController();
                    return null;
                },
                serviceType => Enumerable.Empty<object>()
            );
        }
    }
}
