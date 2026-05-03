using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Clientes.Commands.UpdateCliente
{
    public class UpdateClienteCommand : IRequest<Cliente>
    {
        public Guid idCliente { get; set; }
        public string nomeCliente { get; set; }
        public string endereco { get; set; }
    }
}
