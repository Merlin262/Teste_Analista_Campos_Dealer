using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TesteCamposDealer.Application.Common;
using TesteCamposDealer.Models;
using TesteCamposDealer.Web.ViewModels;

namespace TesteCamposDealer.Mappers
{
    public static class ViewModelMappers
    {
        public static ClienteViewModel ToViewModel(this Cliente c) => new ClienteViewModel
        {
            idCliente = c.idCliente,
            nomeCliente = c.nomeCliente,
            endereco = c.endereco,
            dthRegistro = c.dthRegistro
        };

        public static ProdutoViewModel ToViewModel(this Produto p) => new ProdutoViewModel
        {
            idProduto = p.idProduto,
            dscProduto = p.dscProduto,
            vlrProduto = p.vlrProduto
        };

        public static VendaViewModel ToViewModel(this Venda v) => new VendaViewModel
        {
            idVenda = v.idVenda,
            idCliente = v.idCliente,
            nomeCliente = v.Cliente?.nomeCliente,
            enderecoCliente = v.Cliente?.endereco,
            vlrTotal = v.vlrTotal,
            dthRegistro = v.dthRegistro,
            itens = v.Itens?.Select(i => new VendaItemViewModel
            {
                idVendaItem = i.idVendaItem,
                idProduto = i.idProduto,
                dscProduto = i.Produto?.dscProduto,
                quantidade = i.quantidade,
                vlrUnitario = i.vlrUnitario,
                vlrTotal = i.vlrTotal
            }).ToList() ?? new System.Collections.Generic.List<VendaItemViewModel>()
        };

        public static PagedResultViewModel<TViewModel> ToPagedViewModel<TModel, TViewModel>(
            this PagedResult<TModel> r,
            System.Func<TModel, TViewModel> map) =>
            new PagedResultViewModel<TViewModel>
            {
                Data = r.Data.Select(map).ToList(),
                Page = r.Page,
                PageSize = r.PageSize,
                Total = r.Total,
                TotalPages = r.TotalPages
            };
    }
}