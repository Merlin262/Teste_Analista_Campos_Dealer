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

namespace TesteCamposDealer.Application.Handlers.Vendas.Commands.CreateVenda
{
    public class CreateVendaHandler : IRequestHandler<CreateVendaCommand, Venda>
    {
        private readonly IUnitOfWork _uow;
        public CreateVendaHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<Venda> Handle(CreateVendaCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _uow.Clientes.GetByIdAsync(request.idCliente);
            if (cliente == null)
                throw new NotFoundException("Cliente", request.idCliente);

            var venda = new Venda
            {
                idCliente = request.idCliente,
                dthRegistro = DateTime.Now,
                Itens = new List<VendaItem>()
            };

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

            _uow.Vendas.Add(venda);
            await _uow.CommitAsync();

            return await _uow.Vendas.GetByIdWithDetailsAsync(venda.idVenda);
        }
    }
}
