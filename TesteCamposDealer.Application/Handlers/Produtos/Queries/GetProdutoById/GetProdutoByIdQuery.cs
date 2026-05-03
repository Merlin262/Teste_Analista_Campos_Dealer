using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Produtos.Queries.GetProdutoById
{
    public class GetProdutoByIdQuery : IRequest<Produto>
    {
        public Guid IdProduto { get; }
        public GetProdutoByIdQuery(Guid idProduto) { IdProduto = idProduto; }
    }
}
