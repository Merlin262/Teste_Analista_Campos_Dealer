using MediatR;
using Medo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Produtos.Commands.UpdateProduto
{
    public class UpdateProdutoHandler : IRequestHandler<UpdateProdutoCommand, Produto>
    {
        private readonly IUnitOfWork _uow;
        public UpdateProdutoHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<Produto> Handle(UpdateProdutoCommand request, CancellationToken cancellationToken)
        {
            var produto = await _uow.Produtos.GetByIdAsync(request.idProduto);
            if (produto == null)
                return null;

            if (produto.vlrProduto != request.vlrProduto)
            {
                _uow.Produtos.AddHistorico(new ProdutoPrecoHistorico
                {
                    idHistorico = Uuid7.NewUuid7(),
                    idProduto = produto.idProduto,
                    vlrAnterior = produto.vlrProduto,
                    vlrNovo = request.vlrProduto,
                    dthAlteracao = DateTime.Now
                });
            }

            produto.dscProduto = request.dscProduto;
            produto.vlrProduto = request.vlrProduto;
            await _uow.CommitAsync();
            return produto;
        }
    }
}
