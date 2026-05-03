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
    public class VendaRepositoryTests
    {
        private readonly Mock<AppDbContext> _ctx;
        private readonly VendaRepository _repo;
        private readonly List<Venda> _vendas;
        private readonly List<VendaItem> _itens;

        private static Guid _idCliente1 = Guid.NewGuid();
        private static Guid _idCliente2 = Guid.NewGuid();

        public VendaRepositoryTests()
        {
            _itens = new List<VendaItem>();
            _vendas = new List<Venda>
            {
                new Venda { idVenda = Guid.NewGuid(), idCliente = _idCliente1, vlrTotal = 300m,
                            dthRegistro = DateTime.Now.AddDays(-2) },
                new Venda { idVenda = Guid.NewGuid(), idCliente = _idCliente1, vlrTotal = 100m,
                            dthRegistro = DateTime.Now.AddDays(-1) },
                new Venda { idVenda = Guid.NewGuid(), idCliente = _idCliente2, vlrTotal = 500m,
                            dthRegistro = DateTime.Now }
            };

            var mockVendaSet = MockDbSetHelper.Create(_vendas);
            mockVendaSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                        .Returns<object[]>(ids => Task.FromResult(
                            _vendas.Find(v => v.idVenda == (Guid)ids[0])));

            var mockItemSet = MockDbSetHelper.Create(_itens);

            _ctx = new Mock<AppDbContext>();
            _ctx.Setup(c => c.Venda).Returns(mockVendaSet.Object);
            _ctx.Setup(c => c.VendaItens).Returns(mockItemSet.Object);
            _repo = new VendaRepository(_ctx.Object);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarVenda_QuandoExiste()
        {
            var esperada = _vendas[0];
            var result = await _repo.GetByIdAsync(esperada.idVenda);
            Assert.Equal(esperada, result);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            var result = await _repo.GetByIdAsync(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public async Task CountAsync_DeveRetornarTotalDeVendas()
        {
            var count = await _repo.CountAsync();
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task GetAllWithDetailsPagedAsync_DeveOrdenarPorData()
        {
            var result = await _repo.GetAllWithDetailsPagedAsync(1, 10);
            Assert.True(result[0].dthRegistro <= result[1].dthRegistro);
            Assert.True(result[1].dthRegistro <= result[2].dthRegistro);
        }

        [Fact]
        public async Task GetAllWithDetailsPagedAsync_DeveRespeitarPaginacao()
        {
            var result = await _repo.GetAllWithDetailsPagedAsync(1, 2);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByClienteAsync_DeveRetornarApenasVendasDoCliente()
        {
            var result = await _repo.GetByClienteAsync(_idCliente1);
            Assert.Equal(2, result.Count);
            Assert.All(result, v => Assert.Equal(_idCliente1, v.idCliente));
        }

        [Fact]
        public async Task GetByClienteAsync_DeveRetornarListaVazia_QuandoClienteSemVendas()
        {
            var result = await _repo.GetByClienteAsync(Guid.NewGuid());
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetRankingAsync_DeveOrdenarPorValorDecrescente()
        {
            var result = await _repo.GetRankingAsync();
            Assert.Equal(500m, result[0].vlrTotal);
            Assert.Equal(300m, result[1].vlrTotal);
            Assert.Equal(100m, result[2].vlrTotal);
        }

        [Fact]
        public void Add_DeveAdicionarVendaNaLista()
        {
            var nova = new Venda { idVenda = Guid.NewGuid() };
            _repo.Add(nova);
            Assert.Contains(nova, _vendas);
        }

        [Fact]
        public void Remove_DeveRemoverVendaDaLista()
        {
            var alvo = _vendas[0];
            _repo.Remove(alvo);
            Assert.DoesNotContain(alvo, _vendas);
        }

        [Fact]
        public void RemoveItens_DeveRemoverItensDoDbSet()
        {
            var item1 = new VendaItem { idVendaItem = Guid.NewGuid() };
            var item2 = new VendaItem { idVendaItem = Guid.NewGuid() };
            _itens.AddRange(new[] { item1, item2 });

            _repo.RemoveItens(new List<VendaItem> { item1, item2 });

            Assert.Empty(_itens);
        }
    }
}
