using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Common;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetAllVendas
{
    public class GetAllVendasQuery : IRequest<PagedResult<Venda>>
    {
        public int Page { get; }
        public GetAllVendasQuery(int page = 1) { Page = page < 1 ? 1 : page; }
    }
}
