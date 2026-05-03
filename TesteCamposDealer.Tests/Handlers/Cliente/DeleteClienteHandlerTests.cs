using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Clientes.Commands.DeleteCliente;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers
{
    public class DeleteClienteHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IClienteRepository> _repo;
        private readonly DeleteClienteHandler _handler;

        public DeleteClienteHandlerTests()
        {
            _repo = new Mock<IClienteRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Clientes).Returns(_repo.Object);
            _uow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
            _handler = new DeleteClienteHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarFalse_QuandoNaoExiste()
        {
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cliente)null);

            var result = await _handler.Handle(new DeleteClienteCommand(Guid.NewGuid()), CancellationToken.None);

            Assert.False(result);
            _repo.Verify(r => r.Remove(It.IsAny<Cliente>()), Times.Never);
            _uow.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
