using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Clientes.Queries.GetClienteById
{
    public class GetClienteByIdHandler : IRequestHandler<GetClienteByIdQuery, Cliente>
    {
        private readonly IUnitOfWork _uow;
        public GetClienteByIdHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<Cliente> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _uow.Clientes.GetByIdAsync(request.IdCliente);
            if (cliente == null)
                throw new NotFoundException("Cliente", request.IdCliente);
            return cliente;
        }
    }
}
