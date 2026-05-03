using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Produtos.Queries.GetProdutoById
{
    public class GetProdutoByIdHandler : IRequestHandler<GetProdutoByIdQuery, Produto>
    {
        private readonly IUnitOfWork _uow;
        public GetProdutoByIdHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<Produto> Handle(GetProdutoByIdQuery request, CancellationToken cancellationToken)
        {
            var produto = await _uow.Produtos.GetByIdAsync(request.IdProduto);
            if (produto == null)
                throw new NotFoundException("Produto", request.IdProduto);
            return produto;
        }
    }
}
