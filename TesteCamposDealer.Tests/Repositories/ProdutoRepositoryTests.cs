using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Infrastructure.Data;
using TesteCamposDealer.Infrastructure.Repositories;
using TesteCamposDealer.Models;
using TesteCamposDealer.Tests.Helpers;
using Xunit;

namespace TesteCamposDealer.Tests.Repositories
{
    public class ProdutoRepositoryTests
    {
        private readonly Mock<AppDbContext> _ctx;
        private readonly ProdutoRepository _repo;
        private readonly List<Produto> _produtos;
        private readonly List<ProdutoPrecoHistorico> _historicos;

        public ProdutoRepositoryTests()
        {
            _produtos = new List<Produto>
            {
                new Produto { idProduto = Guid.NewGuid(), dscProduto = "Cadeira",  vlrProduto = 200m },
                new Produto { idProduto = Guid.NewGuid(), dscProduto = "Mesa",     vlrProduto = 500m },
                new Produto { idProduto = Guid.NewGuid(), dscProduto = "Teclado",  vlrProduto = 150m }
            };
            _historicos = new List<ProdutoPrecoHistorico>();

            var mockSet = MockDbSetHelper.Create(_produtos);
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                   .Returns<object[]>(ids => Task.FromResult(
                       _produtos.Find(p => p.idProduto == (Guid)ids[0])));

            var mockHistSet = MockDbSetHelper.Create(_historicos);

            _ctx = new Mock<AppDbContext>();
            _ctx.Setup(c => c.Produto).Returns(mockSet.Object);
            _ctx.Setup(c => c.ProdutoPrecoHistoricos).Returns(mockHistSet.Object);
            _repo = new ProdutoRepository(_ctx.Object);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarProduto_QuandoExiste()
        {
            var esperado = _produtos[1];
            var result = await _repo.GetByIdAsync(esperado.idProduto);
            Assert.Equal(esperado, result);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            var result = await _repo.GetByIdAsync(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllPagedAsync_DeveOrdenarPorDescricao()
        {
            var result = await _repo.GetAllPagedAsync(1, 10);
            Assert.Equal("Cadeira", result[0].dscProduto);
            Assert.Equal("Mesa", result[1].dscProduto);
            Assert.Equal("Teclado", result[2].dscProduto);
        }

        [Fact]
        public async Task GetAllPagedAsync_DeveRespeitarPaginacao()
        {
            var result = await _repo.GetAllPagedAsync(1, 2);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task CountAsync_DeveRetornarTotalDeProdutos()
        {
            var count = await _repo.CountAsync();
            Assert.Equal(3, count);
        }

        [Fact]
        public void Add_DeveAdicionarProdutoNaLista()
        {
            var novo = new Produto { idProduto = Guid.NewGuid(), dscProduto = "Monitor" };
            _repo.Add(novo);
            Assert.Contains(novo, _produtos);
        }

        [Fact]
        public void Remove_DeveRemoverProdutoDaLista()
        {
            var alvo = _produtos[0];
            _repo.Remove(alvo);
            Assert.DoesNotContain(alvo, _produtos);
        }

        [Fact]
        public void AddHistorico_DeveAdicionarRegistroDeHistorico()
        {
            var historico = new ProdutoPrecoHistorico
            {
                idHistorico = Guid.NewGuid(),
                idProduto = _produtos[0].idProduto,
                vlrAnterior = 200m,
                vlrNovo = 250m
            };

            _repo.AddHistorico(historico);

            Assert.Contains(historico, _historicos);
        }
    }
}
