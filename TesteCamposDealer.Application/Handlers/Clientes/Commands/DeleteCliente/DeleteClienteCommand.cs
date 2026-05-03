using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCamposDealer.Application.Handlers.Clientes.Commands.DeleteCliente
{
    public class DeleteClienteCommand : IRequest<bool>
    {
        public Guid IdCliente { get; }
        public DeleteClienteCommand(Guid idCliente) { IdCliente = idCliente; }
    }
}
