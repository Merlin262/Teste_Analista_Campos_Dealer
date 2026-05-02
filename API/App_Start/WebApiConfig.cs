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
            //Rotas da API.
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional });

            config.Routes.MapHttpRoute(
            name: "ClienteGetByIdApiRoute",
            routeTemplate: "api/cliente/getbyid/{idCliente}/",
            defaults: new { controller = "Cliente", action = "GetById" });

            config.Routes.MapHttpRoute(
            name: "ClienteDeleteByIdApiRoute",
            routeTemplate: "api/cliente/deletebyid/{idCliente}/",
            defaults: new { controller = "Cliente", action = "DeleteById" });

            config.Routes.MapHttpRoute(
            name: "ClientePutByIdApiRoute",
            routeTemplate: "api/cliente/putbyid/{idCliente}/",
            defaults: new { controller = "Cliente", action = "PutById" });



            config.Routes.MapHttpRoute(
            name: "ProdutoGetByIdApiRoute",
            routeTemplate: "api/produto/getbyid/{idProduto}/",
            defaults: new { controller = "Produto", action = "GetById" });

            config.Routes.MapHttpRoute(
            name: "ProdutoDeleteByIdApiRoute",
            routeTemplate: "api/produto/deletebyid/{idProduto}/",
            defaults: new { controller = "Produto", action = "DeleteById" });

            config.Routes.MapHttpRoute(
            name: "ProdutoPutByIdApiRoute",
            routeTemplate: "api/produto/putbyid/{idProduto}/",
            defaults: new { controller = "Produto", action = "PutById" });


            config.Routes.MapHttpRoute(
            name: "VendaGetByIdApiRoute",
            routeTemplate: "api/venda/getbyid/{idVenda}/",
            defaults: new { controller = "Venda", action = "GetById" });

            config.Routes.MapHttpRoute(
            name: "VendaDeleteByIdApiRoute",
            routeTemplate: "api/venda/deletebyid/{idVenda}/",
            defaults: new { controller = "Venda", action = "DeleteById" });

            config.Routes.MapHttpRoute(
            name: "VendasPutByIdApiRoute",
            routeTemplate: "api/venda/putbyid/{idVenda}/",
            defaults: new { controller = "Venda", action = "PutById" });
        }
    }
}
