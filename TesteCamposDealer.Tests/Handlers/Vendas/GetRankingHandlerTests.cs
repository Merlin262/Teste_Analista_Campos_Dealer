using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Vendas.Queries.GetRanking;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers.Vendas
{
    public class GetRankingHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IVendaRepository> _repo;
        private readonly GetRankingHandler _handler;

        public GetRankingHandlerTests()
        {
            _repo = new Mock<IVendaRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Vendas).Returns(_repo.Object);
            _handler = new GetRankingHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarRanking()
        {
            var ranking = new List<Venda>
            {
                new Venda { vlrTotal = 500m },
                new Venda { vlrTotal = 300m },
                new Venda { vlrTotal = 100m }
            };
            _repo.Setup(r => r.GetRankingAsync()).ReturnsAsync(ranking);

            var result = await _handler.Handle(new GetRankingQuery(), CancellationToken.None);

            Assert.Equal(3, result.Count);
            Assert.Equal(500m, result[0].vlrTotal);
        }
    }
}
