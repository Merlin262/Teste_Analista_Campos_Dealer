using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Produtos.Queries.GetAllProdutos;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers
{
    public class GetAllProdutosHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IProdutoRepository> _repo;
        private readonly GetAllProdutosHandler _handler;

        public GetAllProdutosHandlerTests()
        {
            _repo = new Mock<IProdutoRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Produtos).Returns(_repo.Object);
            _handler = new GetAllProdutosHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarPagedResult()
        {
            var produtos = new List<Produto> { new Produto(), new Produto(), new Produto() };
            _repo.Setup(r => r.CountAsync()).ReturnsAsync(3);
            _repo.Setup(r => r.GetAllPagedAsync(1, 10)).ReturnsAsync(produtos);

            var result = await _handler.Handle(new GetAllProdutosQuery(1), CancellationToken.None);

            Assert.Equal(3, result.Total);
            Assert.Equal(3, result.Data.Count);
            Assert.Equal(1, result.TotalPages);
        }

        [Fact]
        public async Task Handle_DeveRetornarListaVazia_QuandoSemProdutos()
        {
            _repo.Setup(r => r.CountAsync()).ReturnsAsync(0);
            _repo.Setup(r => r.GetAllPagedAsync(1, 10)).ReturnsAsync(new List<Produto>());

            var result = await _handler.Handle(new GetAllProdutosQuery(1), CancellationToken.None);

            Assert.Empty(result.Data);
            Assert.Equal(0, result.Total);
        }
    }
}
