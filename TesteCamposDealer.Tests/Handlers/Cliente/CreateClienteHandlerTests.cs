using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Clientes.Commands;
using TesteCamposDealer.Application.Handlers.Clientes.Commands.DeleteCliente;
using TesteCamposDealer.Application.Handlers.Clientes.Commands.UpdateCliente;
using TesteCamposDealer.Application.Handlers.Clientes.Queries.GetAllClientes;
using TesteCamposDealer.Application.Handlers.Clientes.Queries.GetClienteById;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers
{
    public class CreateClienteHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IClienteRepository> _repo;
        private readonly CreateClienteHandler _handler;

        public CreateClienteHandlerTests()
        {
            _repo = new Mock<IClienteRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Clientes).Returns(_repo.Object);
            _uow.Setup(u => u.CommitAsync()).ReturnsAsync(1);
            _handler = new CreateClienteHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarClienteComPropriedadesCorretas()
        {
            var cmd = new CreateClienteCommand { nomeCliente = "João Silva", endereco = "Rua A, 10" };

            var result = await _handler.Handle(cmd, CancellationToken.None);

            Assert.Equal("João Silva", result.nomeCliente);
            Assert.Equal("Rua A, 10", result.endereco);
        }

        [Fact]
        public async Task Handle_DeveAdicionarClienteEConfirmar()
        {
            var cmd = new CreateClienteCommand { nomeCliente = "Maria", endereco = "Av B" };

            await _handler.Handle(cmd, CancellationToken.None);

            _repo.Verify(r => r.Add(It.IsAny<Cliente>()), Times.Once);
            _uow.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_DeveDefinirDthRegistro()
        {
            var antes = DateTime.Now;
            var cmd = new CreateClienteCommand { nomeCliente = "X", endereco = "Y" };

            var result = await _handler.Handle(cmd, CancellationToken.None);

            Assert.True(result.dthRegistro >= antes);
        }
    }
}
