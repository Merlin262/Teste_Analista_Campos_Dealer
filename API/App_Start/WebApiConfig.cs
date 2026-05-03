using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TesteCamposDealer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Rotas da API baseadas em WebAPI foram removidas para não conflitar com o roteamento do MVC
            // Já que os Controllers (ClienteController, ProdutoController, VendaController) herdam de Controller
            // e utilizam os atributos do [RoutePrefix("api/...")]
            config.MapHttpAttributeRoutes();
        }
    }
}
