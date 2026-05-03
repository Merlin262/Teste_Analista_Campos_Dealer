using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Produtos.Commands.UpdateProduto
{
    public class UpdateProdutoCommand : IRequest<Produto>
    {
        public Guid idProduto { get; set; }
        public string dscProduto { get; set; }
        public decimal vlrProduto { get; set; }
    }
}
