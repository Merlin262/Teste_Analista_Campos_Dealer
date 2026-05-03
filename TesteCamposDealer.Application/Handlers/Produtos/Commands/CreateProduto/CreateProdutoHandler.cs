using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Produtos.Commands
{
    public class CreateProdutoHandler : IRequestHandler<CreateProdutoCommand, Produto>
    {
        private readonly IUnitOfWork _uow;
        public CreateProdutoHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<Produto> Handle(CreateProdutoCommand request, CancellationToken cancellationToken)
        {
            var produto = new Produto
            {
                dscProduto = request.dscProduto,
                vlrProduto = request.vlrProduto
            };

            _uow.Produtos.Add(produto);
            await _uow.CommitAsync();
            return produto;
        }
    }
}
