using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Vendas.Queries.GetAllVendas;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers.Vendas
{
    public class GetAllVendasHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IVendaRepository> _repo;
        private readonly GetAllVendasHandler _handler;

        public GetAllVendasHandlerTests()
        {
            _repo = new Mock<IVendaRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Vendas).Returns(_repo.Object);
            _handler = new GetAllVendasHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarPagedResult()
        {
            var vendas = new List<Venda> { new Venda(), new Venda() };
            _repo.Setup(r => r.CountAsync()).ReturnsAsync(2);
            _repo.Setup(r => r.GetAllWithDetailsPagedAsync(1, 10)).ReturnsAsync(vendas);

            var result = await _handler.Handle(new GetAllVendasQuery(1), CancellationToken.None);

            Assert.Equal(2, result.Total);
            Assert.Equal(2, result.Data.Count);
        }
    }
}
