using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Vendas.Commands.DeleteVenda;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers.Vendas
{
    public class DeleteVendaHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IVendaRepository> _repo;
        private readonly DeleteVendaHandler _handler;

        public DeleteVendaHandlerTests()
        {
            _repo = new Mock<IVendaRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Vendas).Returns(_repo.Object);
            _uow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
            _handler = new DeleteVendaHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarTrue_QuandoExiste()
        {
            var id = Guid.NewGuid();
            var venda = new Venda { idVenda = id };
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(venda);

            var result = await _handler.Handle(new DeleteVendaCommand(id), CancellationToken.None);

            Assert.True(result);
            _repo.Verify(r => r.Remove(venda), Times.Once);
            _uow.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalse_QuandoNaoExiste()
        {
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Venda)null);

            var result = await _handler.Handle(
                new DeleteVendaCommand(Guid.NewGuid()), CancellationToken.None);

            Assert.False(result);
            _uow.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
