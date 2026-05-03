using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Produtos.Commands;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers
{
    public class CreateProdutoHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IProdutoRepository> _repo;
        private readonly CreateProdutoHandler _handler;

        public CreateProdutoHandlerTests()
        {
            _repo = new Mock<IProdutoRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Produtos).Returns(_repo.Object);
            _uow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
            _handler = new CreateProdutoHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarProdutoComPropriedadesCorretas()
        {
            var cmd = new CreateProdutoCommand { dscProduto = "Produto A", vlrProduto = 99.90m };
            var result = await _handler.Handle(cmd, CancellationToken.None);

            Assert.Equal("Produto A", result.dscProduto);
            Assert.Equal(99.90m, result.vlrProduto);
        }

        [Fact]
        public async Task Handle_DeveAdicionarProdutoEConfirmar()
        {
            var cmd = new CreateProdutoCommand { dscProduto = "X", vlrProduto = 1m };

            await _handler.Handle(cmd, CancellationToken.None);

            _repo.Verify(r => r.Add(It.IsAny<Produto>()), Times.Once);
            _uow.Verify(u => u.CommitAsync(), Times.Once);
        }
    }
}
