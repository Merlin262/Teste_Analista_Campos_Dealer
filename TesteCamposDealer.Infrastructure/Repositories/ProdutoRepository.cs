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
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _ctx;
        public ProdutoRepository(AppDbContext ctx) { _ctx = ctx; }

        public Task<Produto> GetByIdAsync(Guid id) =>
            _ctx.Produto.FindAsync(id);

        public Task<List<Produto>> GetAllPagedAsync(int page, int pageSize) =>
            _ctx.Produto
                .OrderBy(p => p.dscProduto)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public Task<int> CountAsync() =>
            _ctx.Produto.CountAsync();

        public void Add(Produto entity) =>
            _ctx.Produto.Add(entity);

        public void Remove(Produto entity) =>
            _ctx.Produto.Remove(entity);

        public void AddHistorico(ProdutoPrecoHistorico historico) =>
            _ctx.ProdutoPrecoHistoricos.Add(historico);
    }
}
