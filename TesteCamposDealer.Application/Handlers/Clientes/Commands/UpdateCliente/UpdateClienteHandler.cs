using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Clientes.Commands.UpdateCliente
{
    public class UpdateClienteHandler : IRequestHandler<UpdateClienteCommand, Cliente>
    {
        private readonly IUnitOfWork _uow;
        public UpdateClienteHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<Cliente> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _uow.Clientes.GetByIdAsync(request.idCliente);
            if (cliente == null)
                throw new NotFoundException("Cliente", request.idCliente);

            cliente.nomeCliente = request.nomeCliente;
            cliente.endereco = request.endereco;
            await _uow.CommitAsync();
            return cliente;
        }
    }
}
