using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Commands.UpdateVenda
{
    public class UpdateVendaHandler : IRequestHandler<UpdateVendaCommand, Venda>
    {
        private readonly IUnitOfWork _uow;
        public UpdateVendaHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<Venda> Handle(UpdateVendaCommand request, CancellationToken cancellationToken)
        {
            var venda = await _uow.Vendas.GetByIdWithItensAsync(request.idVenda);
            if (venda == null)
                return null;

            _uow.Vendas.RemoveItens(venda.Itens.ToList());
            venda.Itens.Clear();

            foreach (var itemReq in request.itens)
            {
                var produto = await _uow.Produtos.GetByIdAsync(itemReq.idProduto);
                if (produto == null)
                    throw new NotFoundException("Produto", itemReq.idProduto);

                venda.Itens.Add(new VendaItem
                {
                    idProduto = produto.idProduto,
                    quantidade = itemReq.quantidade,
                    vlrUnitario = produto.vlrProduto,
                    vlrTotal = produto.vlrProduto * itemReq.quantidade
                });
            }

            venda.vlrTotal = venda.Itens.Sum(i => i.vlrTotal);
            await _uow.CommitAsync();

            return await _uow.Vendas.GetByIdWithDetailsAsync(request.idVenda);
        }
    }
}
