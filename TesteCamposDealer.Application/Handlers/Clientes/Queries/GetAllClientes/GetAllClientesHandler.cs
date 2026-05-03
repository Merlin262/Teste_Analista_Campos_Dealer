using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Common;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Clientes.Queries.GetAllClientes
{
    public class GetAllClientesHandler : IRequestHandler<GetAllClientesQuery, PagedResult<Cliente>>
    {
        private const int PageSize = 10;
        private readonly IUnitOfWork _uow;
        public GetAllClientesHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<PagedResult<Cliente>> Handle(GetAllClientesQuery request, CancellationToken cancellationToken)
        {
            var total = await _uow.Clientes.CountAsync();
            var data = await _uow.Clientes.GetAllPagedAsync(request.Page, PageSize);

            return new PagedResult<Cliente>
            {
                Data = data,
                Page = request.Page,
                PageSize = PageSize,
                Total = total
            };
        }
    }
}
