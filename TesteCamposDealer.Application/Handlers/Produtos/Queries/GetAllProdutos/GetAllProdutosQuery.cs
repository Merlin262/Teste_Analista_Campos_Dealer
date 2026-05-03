using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Common;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Produtos.Queries.GetAllProdutos
{
    public class GetAllProdutosQuery : IRequest<PagedResult<Produto>>
    {
        public int Page { get; }
        public GetAllProdutosQuery(int page = 1) { Page = page < 1 ? 1 : page; }
    }
}
