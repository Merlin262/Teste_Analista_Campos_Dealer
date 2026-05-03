using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendasByCliente
{
    public class GetVendasByClienteQuery : IRequest<List<Venda>>
    {
        public Guid IdCliente { get; }
        public GetVendasByClienteQuery(Guid idCliente) { IdCliente = idCliente; }
    }
}
