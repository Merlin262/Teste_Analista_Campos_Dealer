using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Clientes.Commands
{
    public class CreateClienteCommand : IRequest<Cliente>
    {
        public string nomeCliente { get; set; }
        public string endereco { get; set; }
    }

    public class CreateClienteHandler : IRequestHandler<CreateClienteCommand, Cliente>
    {
        private readonly IUnitOfWork _uow;
        public CreateClienteHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<Cliente> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = new Cliente
            {
                nomeCliente = request.nomeCliente,
                endereco = request.endereco,
                dthRegistro = DateTime.Now
            };

            _uow.Clientes.Add(cliente);
            await _uow.CommitAsync();
            return cliente;
        }
    }
}
