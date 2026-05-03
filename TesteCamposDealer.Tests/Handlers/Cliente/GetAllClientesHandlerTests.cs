using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Clientes.Queries.GetAllClientes;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers
{
    public class GetAllClientesHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IClienteRepository> _repo;
        private readonly GetAllClientesHandler _handler;

        public GetAllClientesHandlerTests()
        {
            _repo = new Mock<IClienteRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Clientes).Returns(_repo.Object);
            _handler = new GetAllClientesHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarPagedResult()
        {
            var clientes = new List<Cliente> { new Cliente(), new Cliente() };
            _repo.Setup(r => r.CountAsync()).ReturnsAsync(2);
            _repo.Setup(r => r.GetAllPagedAsync(1, 10)).ReturnsAsync(clientes);

            var result = await _handler.Handle(new GetAllClientesQuery(1), CancellationToken.None);

            Assert.Equal(2, result.Total);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(1, result.Page);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public async Task Handle_DeveNormalizarPaginaMenorQueUm()
        {
            _repo.Setup(r => r.CountAsync()).ReturnsAsync(0);
            _repo.Setup(r => r.GetAllPagedAsync(1, 10)).ReturnsAsync(new List<Cliente>());

            var result = await _handler.Handle(new GetAllClientesQuery(0), CancellationToken.None);

            Assert.Equal(1, result.Page);
        }

        [Fact]
        public async Task Handle_DeveCalcularTotalPaginasCorretamente()
        {
            _repo.Setup(r => r.CountAsync()).ReturnsAsync(25);
            _repo.Setup(r => r.GetAllPagedAsync(It.IsAny<int>(), It.IsAny<int>()))
                 .ReturnsAsync(new List<Cliente>());

            var result = await _handler.Handle(new GetAllClientesQuery(1), CancellationToken.None);

            Assert.Equal(3, result.TotalPages);
        }
    }
}
