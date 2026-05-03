using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Clientes.Queries.GetClienteById
{
    public class GetClienteByIdQuery : IRequest<Cliente>
    {
        public Guid IdCliente { get; }
        public GetClienteByIdQuery(Guid idCliente) { IdCliente = idCliente; }
    }
}
