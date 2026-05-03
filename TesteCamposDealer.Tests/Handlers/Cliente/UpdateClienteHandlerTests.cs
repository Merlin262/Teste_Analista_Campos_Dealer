using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Application.Handlers.Clientes.Commands.UpdateCliente;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers
{
    public class UpdateClienteHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IClienteRepository> _repo;
        private readonly UpdateClienteHandler _handler;

        public UpdateClienteHandlerTests()
        {
            _repo = new Mock<IClienteRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Clientes).Returns(_repo.Object);
            _uow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
            _handler = new UpdateClienteHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveAtualizarERetornarCliente_QuandoExiste()
        {
            var id = Guid.NewGuid();
            var cliente = new Cliente { idCliente = id, nomeCliente = "Antigo", endereco = "Rua Velha" };
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(cliente);

            var cmd = new UpdateClienteCommand { idCliente = id, nomeCliente = "Novo Nome", endereco = "Rua Nova" };
            var result = await _handler.Handle(cmd, CancellationToken.None);

            Assert.Equal("Novo Nome", result.nomeCliente);
            Assert.Equal("Rua Nova", result.endereco);
            _uow.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveLancarNotFoundException_QuandoNaoExiste()
        {
            _repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Cliente)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(new UpdateClienteCommand { idCliente = Guid.NewGuid() }, CancellationToken.None));

            _uow.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
