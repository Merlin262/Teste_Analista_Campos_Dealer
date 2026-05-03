using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Handlers.Produtos.Queries.GetProdutoById;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;
using Xunit;

namespace TesteCamposDealer.Tests.Handlers
{
    public class GetProdutoByIdHandlerTests
    {
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IProdutoRepository> _repo;
        private readonly GetProdutoByIdHandler _handler;

        public GetProdutoByIdHandlerTests()
        {
            _repo = new Mock<IProdutoRepository>();
            _uow = new Mock<IUnitOfWork>();
            _uow.Setup(u => u.Produtos).Returns(_repo.Object);
            _handler = new GetProdutoByIdHandler(_uow.Object);
        }

        [Fact]
        public async Task Handle_DeveRetornarProduto_QuandoEncontrado()
        {
            var id = Guid.NewGuid();
            var produto = new Produto { idProduto = id };
            _repo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(produto);

            var result = await _handler.Handle(new GetProdutoByIdQuery(id), CancellationToken.None);

            Assert.Equal(produto, result);
        }
    }
}
