using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendasByCliente
{
    public class GetVendasByClienteHandler : IRequestHandler<GetVendasByClienteQuery, List<Venda>>
    {
        private readonly IUnitOfWork _uow;
        public GetVendasByClienteHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<List<Venda>> Handle(GetVendasByClienteQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _uow.Clientes.GetByIdAsync(request.IdCliente);
            if (cliente == null)
                throw new NotFoundException("Cliente", request.IdCliente);

            return await _uow.Vendas.GetByClienteAsync(request.IdCliente);
        }
    }
}
