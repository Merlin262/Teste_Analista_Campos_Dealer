using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Domain.Interface;

namespace TesteCamposDealer.Application.Handlers.Produtos.Commands.DeleteProduto
{
    public class DeleteProdutoHandler : IRequestHandler<DeleteProdutoCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        public DeleteProdutoHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<bool> Handle(DeleteProdutoCommand request, CancellationToken cancellationToken)
        {
            var produto = await _uow.Produtos.GetByIdAsync(request.IdProduto);
            if (produto == null)
                return false;

            if (await _uow.Vendas.HasItensByProdutoAsync(request.IdProduto))
                throw new InvalidOperationException("Produto está vinculado a vendas e não pode ser excluído.");

            _uow.Produtos.Remove(produto);
            await _uow.CommitAsync();
            return true;
        }
    }
}
