using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Infrastructure.Data;
using TesteCamposDealer.Infrastructure.Repositories;
using TesteCamposDealer.Models;
using TesteCamposDealer.Tests.Helpers;
using Xunit;

namespace TesteCamposDealer.Tests.Repositories
{
    public class ClienteRepositoryTests
    {
        private readonly Mock<AppDbContext> _ctx;
        private readonly ClienteRepository _repo;
        private readonly List<Cliente> _clientes;

        public ClienteRepositoryTests()
        {
            _clientes = new List<Cliente>
            {
                new Cliente { idCliente = Guid.NewGuid(), nomeCliente = "Ana",   endereco = "Rua 1" },
                new Cliente { idCliente = Guid.NewGuid(), nomeCliente = "Bruno", endereco = "Rua 2" },
                new Cliente { idCliente = Guid.NewGuid(), nomeCliente = "Carlos", endereco = "Rua 3" }
            };

            var mockSet = MockDbSetHelper.Create(_clientes);
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                   .Returns<object[]>(ids => Task.FromResult(
                       _clientes.Find(c => c.idCliente == (Guid)ids[0])));

            _ctx = new Mock<AppDbContext>();
            _ctx.Setup(c => c.Cliente).Returns(mockSet.Object);
            _repo = new ClienteRepository(_ctx.Object);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarCliente_QuandoExiste()
        {
            var esperado = _clientes[0];
            var result = await _repo.GetByIdAsync(esperado.idCliente);
            Assert.Equal(esperado, result);
        }

        [Fact]
        public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            var result = await _repo.GetByIdAsync(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllPagedAsync_DeveRetornarPaginaCorreta()
        {
            var result = await _repo.GetAllPagedAsync(1, 2);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllPagedAsync_DeveOrdenarPorNome()
        {
            var result = await _repo.GetAllPagedAsync(1, 10);
            Assert.Equal("Ana", result[0].nomeCliente);
            Assert.Equal("Bruno", result[1].nomeCliente);
            Assert.Equal("Carlos", result[2].nomeCliente);
        }

        [Fact]
        public async Task GetAllPagedAsync_DeveRespeitarOffset()
        {
            var result = await _repo.GetAllPagedAsync(2, 2);
            Assert.Single(result);
            Assert.Equal("Carlos", result[0].nomeCliente);
        }

        [Fact]
        public async Task CountAsync_DeveRetornarTotalDeClientes()
        {
            var count = await _repo.CountAsync();
            Assert.Equal(3, count);
        }

        [Fact]
        public void Add_DeveAdicionarClienteNaLista()
        {
            var novo = new Cliente { idCliente = Guid.NewGuid(), nomeCliente = "Novo" };
            _repo.Add(novo);
            Assert.Contains(novo, _clientes);
        }

        [Fact]
        public void Remove_DeveRemoverClienteDaLista()
        {
            var alvo = _clientes[0];
            _repo.Remove(alvo);
            Assert.DoesNotContain(alvo, _clientes);
        }
    }
}
