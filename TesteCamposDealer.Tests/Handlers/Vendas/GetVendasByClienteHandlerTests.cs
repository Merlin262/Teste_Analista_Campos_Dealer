using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendasByCliente;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers.Vendas
{
    public class GetVendasByClienteHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IClienteRepository> _clienteRepo;
        private readonly Mock<IVendaRepository> _vendaRepo;
        private readonly GetVendasByClienteHandler _handler;

        public GetVendasByClienteHandlerTests()
        {
            _clienteRepo = new Mock<IClienteRepository>();
            _vendaRepo = new Mock<IVendaRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Clientes).Returns(_clienteRepo.Object);
            _uow.Setup(u => u.Vendas).Returns(_vendaRepo.Object);
            _handler = new GetVendasByClienteHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarVendas_QuandoClienteExiste()
        {
            var idCliente = Guid.NewGuid();
            var vendas = new List<Venda> { new Venda(), new Venda() };
            _clienteRepo.Setup(r => r.GetByIdAsync(idCliente))
                        .ReturnsAsync(new Cliente { idCliente = idCliente });
            _vendaRepo.Setup(r => r.GetByClienteAsync(idCliente)).ReturnsAsync(vendas);

            var result = await _handler.Handle(
                new GetVendasByClienteQuery(idCliente), CancellationToken.None);

            Assert.Equal(2, result.Count);
        }

    }
}
