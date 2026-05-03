using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Dto;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Application.Handlers.Vendas.Commands.UpdateVenda;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers.Vendas
{
    public class UpdateVendaHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IProdutoRepository> _produtoRepo;
        private readonly Mock<IVendaRepository> _vendaRepo;
        private readonly UpdateVendaHandler _handler;

        public UpdateVendaHandlerTests()
        {
            _produtoRepo = new Mock<IProdutoRepository>();
            _vendaRepo = new Mock<IVendaRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Produtos).Returns(_produtoRepo.Object);
            _uow.Setup(u => u.Vendas).Returns(_vendaRepo.Object);
            _uow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
            _handler = new UpdateVendaHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveLancarNotFoundException_QuandoVendaNaoExiste()
        {
            _vendaRepo.Setup(r => r.GetByIdWithItensAsync(It.IsAny<Guid>())).ReturnsAsync((Venda)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(new UpdateVendaCommand { idVenda = Guid.NewGuid(), itens = new List<VendaItemRequest>() },
                    CancellationToken.None));

            _uow.Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveRemoverItensAntigosEAdicionarNovos()
        {
            var idVenda = Guid.NewGuid();
            var idProduto = Guid.NewGuid();
            var itemAntigo = new VendaItem { idVendaItem = Guid.NewGuid() };
            var venda = new Venda { idVenda = idVenda, Itens = new List<VendaItem> { itemAntigo } };

            _vendaRepo.Setup(r => r.GetByIdWithItensAsync(idVenda)).ReturnsAsync(venda);
            _produtoRepo.Setup(r => r.GetByIdAsync(idProduto))
                        .ReturnsAsync(new Produto { idProduto = idProduto, vlrProduto = 30m });
            _vendaRepo.Setup(r => r.GetByIdWithDetailsAsync(idVenda)).ReturnsAsync(venda);

            await _handler.Handle(new UpdateVendaCommand
            {
                idVenda = idVenda,
                itens = new List<VendaItemRequest>
                {
                    new VendaItemRequest { idProduto = idProduto, quantidade = 2 }
                }
            }, CancellationToken.None);

            _vendaRepo.Verify(r => r.RemoveItens(It.IsAny<List<VendaItem>>()), Times.Once);
            _uow.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRecalcularTotalCorreto()
        {
            var idVenda = Guid.NewGuid();
            var idProduto = Guid.NewGuid();
            var venda = new Venda { idVenda = idVenda, Itens = new List<VendaItem>() };

            _vendaRepo.Setup(r => r.GetByIdWithItensAsync(idVenda)).ReturnsAsync(venda);
            _produtoRepo.Setup(r => r.GetByIdAsync(idProduto))
                        .ReturnsAsync(new Produto { idProduto = idProduto, vlrProduto = 15m });
            _vendaRepo.Setup(r => r.GetByIdWithDetailsAsync(idVenda)).ReturnsAsync(venda);

            await _handler.Handle(new UpdateVendaCommand
            {
                idVenda = idVenda,
                itens = new List<VendaItemRequest>
                {
                    new VendaItemRequest { idProduto = idProduto, quantidade = 4 } // 60
                }
            }, CancellationToken.None);

            Assert.Equal(60m, venda.vlrTotal);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoProdutoNaoEncontrado()
        {
            var idVenda = Guid.NewGuid();
            _vendaRepo.Setup(r => r.GetByIdWithItensAsync(idVenda))
                      .ReturnsAsync(new Venda { Itens = new List<VendaItem>() });
            _produtoRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Produto)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(new UpdateVendaCommand
                {
                    idVenda = idVenda,
                    itens = new List<VendaItemRequest> { new VendaItemRequest { idProduto = Guid.NewGuid() } }
                }, CancellationToken.None));
        }
    }
}
