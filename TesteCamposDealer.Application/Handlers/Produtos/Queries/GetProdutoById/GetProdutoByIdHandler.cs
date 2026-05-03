using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Produtos.Queries.GetProdutoById
{
    public class GetProdutoByIdHandler : IRequestHandler<GetProdutoByIdQuery, Produto>
    {
        private readonly IUnitOfWork _uow;
        public GetProdutoByIdHandler(IUnitOfWork uow) { _uow = uow; }

        public Task<Produto> Handle(GetProdutoByIdQuery request, CancellationToken cancellationToken)
            => _uow.Produtos.GetByIdAsync(request.IdProduto);
    }
}
