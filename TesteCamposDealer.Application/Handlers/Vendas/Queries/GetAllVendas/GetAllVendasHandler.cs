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

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetAllVendas
{
    public class GetAllVendasHandler : IRequestHandler<GetAllVendasQuery, PagedResult<Venda>>
    {
        private const int PageSize = 10;
        private readonly IUnitOfWork _uow;
        public GetAllVendasHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<PagedResult<Venda>> Handle(GetAllVendasQuery request, CancellationToken cancellationToken)
        {
            var total = await _uow.Vendas.CountAsync();
            var data = await _uow.Vendas.GetAllWithDetailsPagedAsync(request.Page, PageSize);

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
