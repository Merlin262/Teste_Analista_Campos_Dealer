using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Application.Handlers.Produtos.Commands.UpdateProduto;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers
{
    public class UpdateProdutoHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IProdutoRepository> _repo;
        private readonly UpdateProdutoHandler _handler;

        public UpdateProdutoHandlerTests()
        {
            _repo = new Mock<IProdutoRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Produtos).Returns(_repo.Object);
            _uow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
            _handler = new UpdateProdutoHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveAtualizarProduto_QuandoExiste()
        {
            var id = Guid.NewGuid();
            var produto = new Produto { idProduto = id, dscProduto = "Antigo", vlrProduto = 10m };
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(produto);

            var result = await _handler.Handle(
                new UpdateProdutoCommand { idProduto = id, dscProduto = "Novo", vlrProduto = 20m },
                CancellationToken.None);

            Assert.Equal("Novo", result.dscProduto);
            Assert.Equal(20m, result.vlrProduto);
        }

        [Fact]
        public async Task Handle_DeveRegistrarHistorico_QuandoPrecoMuda()
        {
            var id = Guid.NewGuid();
            var produto = new Produto { idProduto = id, vlrProduto = 10m };
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(produto);

            await _handler.Handle(
                new UpdateProdutoCommand { idProduto = id, dscProduto = "X", vlrProduto = 20m },
                CancellationToken.None);

            _repo.Verify(r => r.AddHistorico(It.Is<ProdutoPrecoHistorico>(
                h => h.vlrAnterior == 10m && h.vlrNovo == 20m)), Times.Once);
        }

        [Fact]
        public async Task Handle_NaoDeveRegistrarHistorico_QuandoPrecoNaoMuda()
        {
            var id = Guid.NewGuid();
            var produto = new Produto { idProduto = id, vlrProduto = 10m };
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(produto);

            await _handler.Handle(
                new UpdateProdutoCommand { idProduto = id, dscProduto = "Novo", vlrProduto = 10m },
                CancellationToken.None);

            _repo.Verify(r => r.AddHistorico(It.IsAny<ProdutoPrecoHistorico>()), Times.Never);
        }

        [Fact]
        public async Task Handle_DeveLancarNotFoundException_QuandoNaoExiste()
        {
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Produto)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(new UpdateProdutoCommand { idProduto = Guid.NewGuid() }, CancellationToken.None));

            _uow.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
