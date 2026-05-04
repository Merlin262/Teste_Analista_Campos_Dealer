using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Common;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendasByCliente
{
    public class GetVendasByClienteHandler : IRequestHandler<GetVendasByClienteQuery, PagedResult<Venda>>
    {
        private const int PageSize = 10;
        private readonly IUnitOfWork _uow;
        public GetVendasByClienteHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<PagedResult<Venda>> Handle(GetVendasByClienteQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _uow.Clientes.GetByIdAsync(request.IdCliente);
            if (cliente == null)
                throw new NotFoundException("Cliente", request.IdCliente);

            var total = await _uow.Vendas.CountByClienteAsync(request.IdCliente);
            var data = await _uow.Vendas.GetByClientePagedAsync(request.IdCliente, request.Page, PageSize);

            return new PagedResult<Venda>
            {
                Data = data,
                Page = request.Page,
                PageSize = PageSize,
                Total = total
            };
        }
    }
}
