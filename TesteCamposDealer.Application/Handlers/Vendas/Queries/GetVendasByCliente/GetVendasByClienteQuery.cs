using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Common;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendasByCliente
{
    public class GetVendasByClienteQuery : IRequest<PagedResult<Venda>>
    {
        public Guid IdCliente { get; }
        public int Page { get; }
        public GetVendasByClienteQuery(Guid idCliente, int page = 1)
        {
            IdCliente = idCliente;
            Page = page < 1 ? 1 : page;
        }
    }
}
