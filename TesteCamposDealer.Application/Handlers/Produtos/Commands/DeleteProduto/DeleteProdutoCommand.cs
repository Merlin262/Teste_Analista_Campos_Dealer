using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCamposDealer.Application.Handlers.Produtos.Commands.DeleteProduto
{
    public class DeleteProdutoCommand : IRequest<bool>
    {
        public Guid IdProduto { get; }
        public DeleteProdutoCommand(Guid idProduto) { IdProduto = idProduto; }
    }
}
