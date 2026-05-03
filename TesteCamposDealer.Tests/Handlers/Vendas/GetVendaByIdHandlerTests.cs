using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendaById;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers.Vendas
{
    public class GetVendaByIdHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IVendaRepository> _repo;
        private readonly GetVendaByIdHandler _handler;

        public GetVendaByIdHandlerTests()
        {
            _repo = new Mock<IVendaRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Vendas).Returns(_repo.Object);
            _handler = new GetVendaByIdHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarVenda_QuandoEncontrada()
        {
            var id = Guid.NewGuid();
            var venda = new Venda { idVenda = id };
            _repo.Setup(r => r.GetByIdWithDetailsAsync(id)).ReturnsAsync(venda);

            var result = await _handler.Handle(new GetVendaByIdQuery(id), CancellationToken.None);

            Assert.Equal(venda, result);
        }

        [Fact]
        public async Task Handle_DeveRetornarNull_QuandoNaoEncontrada()
        {
            _repo.Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<Guid>())).ReturnsAsync((Venda)null);

            var result = await _handler.Handle(
                new GetVendaByIdQuery(Guid.NewGuid()), CancellationToken.None);

            Assert.Null(result);
        }
    }
}
