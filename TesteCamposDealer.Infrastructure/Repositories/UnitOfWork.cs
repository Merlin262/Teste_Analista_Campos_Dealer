using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Infrastructure.Data;
using TesteCamposDealer.Domain.Interface;

namespace TesteCamposDealer.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _ctx;

        public IClienteRepository Clientes { get; }
        public IProdutoRepository Produtos { get; }
        public IVendaRepository Vendas { get; }

        public UnitOfWork(AppDbContext ctx)
        {
            _ctx = ctx;
            Clientes = new ClienteRepository(ctx);
            Produtos = new ProdutoRepository(ctx);
            Vendas = new VendaRepository(ctx);
        }

        public Task<int> CommitAsync() => _ctx.SaveChangesAsync();

        public void Dispose() => _ctx.Dispose();
    }
}
