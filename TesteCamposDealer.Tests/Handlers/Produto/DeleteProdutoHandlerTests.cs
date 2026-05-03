using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Produtos.Commands.DeleteProduto;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers
{
    public class DeleteProdutoHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IProdutoRepository> _repo;
        private readonly DeleteProdutoHandler _handler;

        public DeleteProdutoHandlerTests()
        {
            _repo = new Mock<IProdutoRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Produtos).Returns(_repo.Object);
            _uow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
            _handler = new DeleteProdutoHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalse_QuandoNaoExiste()
        {
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Produto)null);

            var result = await _handler.Handle(
                new DeleteProdutoCommand(Guid.NewGuid()), CancellationToken.None);

            Assert.False(result);
            _uow.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
