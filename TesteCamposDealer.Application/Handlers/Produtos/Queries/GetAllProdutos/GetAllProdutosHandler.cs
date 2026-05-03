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

namespace TesteCamposDealer.Application.Handlers.Produtos.Queries.GetAllProdutos
{
    public class GetAllProdutosHandler : IRequestHandler<GetAllProdutosQuery, PagedResult<Produto>>
    {
        private const int PageSize = 10;
        private readonly IUnitOfWork _uow;
        public GetAllProdutosHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<PagedResult<Produto>> Handle(GetAllProdutosQuery request, CancellationToken cancellationToken)
        {
            var total = await _uow.Produtos.CountAsync();
            var data = await _uow.Produtos.GetAllPagedAsync(request.Page, PageSize);

            return new PagedResult<Produto>
            {
                Data = data,
                Page = request.Page,
                PageSize = PageSize,
                Total = total
            };
        }
    }
}
