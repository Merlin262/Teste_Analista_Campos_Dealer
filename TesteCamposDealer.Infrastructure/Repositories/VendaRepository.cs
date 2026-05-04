using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Infrastructure.Data;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Infrastructure.Repositories
{
    public class VendaRepository : IVendaRepository
    {
        private readonly AppDbContext _ctx;
        public VendaRepository(AppDbContext ctx) { _ctx = ctx; }

        public Task<Venda> GetByIdAsync(Guid id) =>
            _ctx.Venda.FindAsync(id);

        public Task<Venda> GetByIdWithDetailsAsync(Guid id) =>
            _ctx.Venda
                .Include(v => v.Cliente)
                .Include(v => v.Itens.Select(i => i.Produto))
                .FirstOrDefaultAsync(v => v.idVenda == id);

        public Task<Venda> GetByIdWithItensAsync(Guid id) =>
            _ctx.Venda
                .Include(v => v.Itens)
                .FirstOrDefaultAsync(v => v.idVenda == id);

        public Task<List<Venda>> GetAllWithDetailsPagedAsync(int page, int pageSize) =>
            _ctx.Venda
                .Include(v => v.Cliente)
                .Include(v => v.Itens.Select(i => i.Produto))
                .OrderBy(v => v.dthRegistro)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public Task<int> CountAsync() =>
            _ctx.Venda.CountAsync();

        public Task<List<Venda>> GetByClienteAsync(Guid idCliente) =>
            _ctx.Venda
                .Include(v => v.Itens.Select(i => i.Produto))
                .Where(v => v.idCliente == idCliente)
                .ToListAsync();

        public Task<List<Venda>> GetByClientePagedAsync(Guid idCliente, int page, int pageSize) =>
            _ctx.Venda
                .Include(v => v.Cliente)
                .Include(v => v.Itens.Select(i => i.Produto))
                .Where(v => v.idCliente == idCliente)
                .OrderBy(v => v.dthRegistro)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public Task<int> CountByClienteAsync(Guid idCliente) =>
            _ctx.Venda.CountAsync(v => v.idCliente == idCliente);

        public Task<List<Venda>> GetRankingAsync() =>
            _ctx.Venda
                .Include(v => v.Cliente)
                .Include(v => v.Itens.Select(i => i.Produto))
                .OrderByDescending(v => v.vlrTotal)
                .ToListAsync();

        public Task<List<Venda>> GetRankingPagedAsync(int page, int pageSize) =>
            _ctx.Venda
                .Include(v => v.Cliente)
                .Include(v => v.Itens.Select(i => i.Produto))
                .OrderByDescending(v => v.vlrTotal)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public Task<bool> HasItensByProdutoAsync(Guid idProduto) =>
            _ctx.VendaItens.AnyAsync(i => i.idProduto == idProduto);

        public void Add(Venda entity) =>
            _ctx.Venda.Add(entity);

        public void Remove(Venda entity) =>
            _ctx.Venda.Remove(entity);

        public void RemoveItens(IEnumerable<VendaItem> itens) =>
            _ctx.VendaItens.RemoveRange(itens);
    }
}
