using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Clientes.Queries.GetClienteById;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers
{
    public class GetClienteByIdHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IClienteRepository> _repo;
        private readonly GetClienteByIdHandler _handler;

        public GetClienteByIdHandlerTests()
        {
            _repo = new Mock<IClienteRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Clientes).Returns(_repo.Object);
            _handler = new GetClienteByIdHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarCliente_QuandoEncontrado()
        {
            var id = Guid.NewGuid();
            var cliente = new Cliente { idCliente = id, nomeCliente = "João" };
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(cliente);

            var result = await _handler.Handle(new GetClienteByIdQuery(id), CancellationToken.None);

            Assert.Equal(cliente, result);
        }

       
    }
}
