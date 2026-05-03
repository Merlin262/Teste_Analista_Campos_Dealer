using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Clientes.Commands.CreateCliente
{
    public class CreateClienteCommand : IRequest<Cliente>
    {
        public string nomeCliente { get; set; }
        public string endereco { get; set; }
    }
}
