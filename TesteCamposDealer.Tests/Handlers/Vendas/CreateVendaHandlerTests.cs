using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Dto;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Application.Handlers.Vendas.Commands.CreateVenda;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers.Vendas
{
    public class CreateVendaHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IClienteRepository> _clienteRepo;
        private readonly Mock<IProdutoRepository> _produtoRepo;
        private readonly Mock<IVendaRepository> _vendaRepo;
        private readonly CreateVendaHandler _handler;

        public CreateVendaHandlerTests()
        {
            _clienteRepo = new Mock<IClienteRepository>();
            _produtoRepo = new Mock<IProdutoRepository>();
            _vendaRepo = new Mock<IVendaRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Clientes).Returns(_clienteRepo.Object);
            _uow.Setup(u => u.Produtos).Returns(_produtoRepo.Object);
            _uow.Setup(u => u.Vendas).Returns(_vendaRepo.Object);
            _uow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
            _handler = new CreateVendaHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveCriarVendaComTotalCalculado()
        {
            var idCliente = Guid.NewGuid();
            var idProduto = Guid.NewGuid();
            var cliente = new Cliente { idCliente = idCliente };
            var produto = new Produto { idProduto = idProduto, vlrProduto = 50m };
            var vendaEsperada = new Venda { idVenda = Guid.NewGuid(), vlrTotal = 100m };

            _clienteRepo.Setup(r => r.GetByIdAsync(idCliente)).ReturnsAsync(cliente);
            _produtoRepo.Setup(r => r.GetByIdAsync(idProduto)).ReturnsAsync(produto);
            _vendaRepo.Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<Guid>())).ReturnsAsync(vendaEsperada);

            var cmd = new CreateVendaCommand
            {
                idCliente = idCliente,
                itens = new List<VendaItemRequest>
                {
                    new VendaItemRequest { idProduto = idProduto, quantidade = 2 }
                }
            };

            var result = await _handler.Handle(cmd, CancellationToken.None);

            Assert.Equal(vendaEsperada, result);
            _vendaRepo.Verify(r => r.Add(It.Is<Venda>(v => v.vlrTotal == 100m)), Times.Once);
            _uow.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoClienteNaoEncontrado()
        {
            _clienteRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cliente)null);

            var cmd = new CreateVendaCommand
            {
                idCliente = Guid.NewGuid(),
                itens = new List<VendaItemRequest> { new VendaItemRequest() }
            };

            await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeveLancarExcecao_QuandoProdutoNaoEncontrado()
        {
            var idCliente = Guid.NewGuid();
            _clienteRepo.Setup(r => r.GetByIdAsync(idCliente))
                        .ReturnsAsync(new Cliente { idCliente = idCliente });
            _produtoRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Produto)null);

            var cmd = new CreateVendaCommand
            {
                idCliente = idCliente,
                itens = new List<VendaItemRequest>
                {
                    new VendaItemRequest { idProduto = Guid.NewGuid(), quantidade = 1 }
                }
            };

            await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeveSomarTotalDeTodosItens()
        {
            var idCliente = Guid.NewGuid();
            var idProd1 = Guid.NewGuid();
            var idProd2 = Guid.NewGuid();

            _clienteRepo.Setup(r => r.GetByIdAsync(idCliente))
                        .ReturnsAsync(new Cliente { idCliente = idCliente });
            _produtoRepo.Setup(r => r.GetByIdAsync(idProd1))
                        .ReturnsAsync(new Produto { idProduto = idProd1, vlrProduto = 10m });
            _produtoRepo.Setup(r => r.GetByIdAsync(idProd2))
                        .ReturnsAsync(new Produto { idProduto = idProd2, vlrProduto = 20m });
            _vendaRepo.Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<Guid>()))
                      .ReturnsAsync(new Venda());

            var cmd = new CreateVendaCommand
            {
                idCliente = idCliente,
                itens = new List<VendaItemRequest>
                {
                    new VendaItemRequest { idProduto = idProd1, quantidade = 2 }, // 20
                    new VendaItemRequest { idProduto = idProd2, quantidade = 3 }  // 60
                }
            };

            await _handler.Handle(cmd, CancellationToken.None);

            _vendaRepo.Verify(r => r.Add(It.Is<Venda>(v => v.vlrTotal == 80m)), Times.Once);
        }
    }
}
