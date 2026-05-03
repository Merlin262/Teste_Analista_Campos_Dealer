using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Dto;
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
                throw new NotFoundException("Venda", request.idVenda);

            var itensAntigos = venda.Itens.ToList();
            _uow.Vendas.RemoveItens(itensAntigos);
            venda.Itens.Clear();

            foreach (var itemReq in request.itens)
            {
                await ProcessarItemRequestAsync(venda, itemReq, itensAntigos);
            }

            venda.vlrTotal = venda.Itens.Sum(i => i.vlrTotal);
            await _uow.CommitAsync();

            return await _uow.Vendas.GetByIdWithDetailsAsync(request.idVenda);
        }

        private async Task ProcessarItemRequestAsync(Venda venda, VendaItemRequest itemReq, List<VendaItem> itensAntigos)
        {
            var produto = await _uow.Produtos.GetByIdAsync(itemReq.idProduto);
            if (produto == null)
                throw new NotFoundException("Produto", itemReq.idProduto);

            var itensAntigosProduto = itensAntigos.Where(i => i.idProduto == itemReq.idProduto).ToList();
            int qtdAntiga = itensAntigosProduto.Sum(i => i.quantidade);

            if (qtdAntiga > 0)
            {
                ProcessarItemExistente(venda, produto, itemReq.quantidade, itensAntigosProduto, qtdAntiga);
            }
            else
            {
                AdicionarNovoItem(venda, produto, itemReq.quantidade);
            }
        }

        private void ProcessarItemExistente(Venda venda, Produto produto, int novaQuantidade, List<VendaItem> itensAntigosProduto, int qtdAntiga)
        {
            if (novaQuantidade > qtdAntiga)
            {
                ManterItensAntigos(venda, itensAntigosProduto);

                int extra = novaQuantidade - qtdAntiga;
                AdicionarNovoItem(venda, produto, extra);
            }
            else
            {
                AjustarQuantidadeItensAntigos(venda, itensAntigosProduto, novaQuantidade);
            }
        }

        private void ManterItensAntigos(Venda venda, List<VendaItem> itensAntigosProduto)
        {
            foreach (var antigo in itensAntigosProduto)
            {
                venda.Itens.Add(new VendaItem
                {
                    idVendaItem = Guid.NewGuid(),
                    idProduto = antigo.idProduto,
                    quantidade = antigo.quantidade,
                    vlrUnitario = antigo.vlrUnitario,
                    vlrTotal = antigo.vlrUnitario * antigo.quantidade
                });
            }
        }

        private void AdicionarNovoItem(Venda venda, Produto produto, int quantidade)
        {
            venda.Itens.Add(new VendaItem
            {
                idVendaItem = Guid.NewGuid(),
                idProduto = produto.idProduto,
                quantidade = quantidade,
                vlrUnitario = produto.vlrProduto,
                vlrTotal = produto.vlrProduto * quantidade
            });
        }

        private void AjustarQuantidadeItensAntigos(Venda venda, List<VendaItem> itensAntigosProduto, int qtdRestante)
        {
            foreach (var antigo in itensAntigosProduto)
            {
                if (qtdRestante <= 0) break;
                int qtd = Math.Min(antigo.quantidade, qtdRestante);
                venda.Itens.Add(new VendaItem
                {
                    idVendaItem = Guid.NewGuid(),
                    idProduto = antigo.idProduto,
                    quantidade = qtd,
                    vlrUnitario = antigo.vlrUnitario,
                    vlrTotal = antigo.vlrUnitario * qtd
                });
                qtdRestante -= qtd;
            }
        }
    }
}
