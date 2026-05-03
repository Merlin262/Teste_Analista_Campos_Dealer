using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Clientes.Queries.GetClienteById
{
    public class GetClienteByIdHandler : IRequestHandler<GetClienteByIdQuery, Cliente>
    {
        private readonly IUnitOfWork _uow;
        public GetClienteByIdHandler(IUnitOfWork uow) { _uow = uow; }

        public Task<Cliente> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
            => _uow.Clientes.GetByIdAsync(request.IdCliente);
    }
}
