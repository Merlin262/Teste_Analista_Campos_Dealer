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
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _ctx;
        public ClienteRepository(AppDbContext ctx) { _ctx = ctx; }

        public Task<Cliente> GetByIdAsync(Guid id) =>
            _ctx.Cliente.FindAsync(id);

        public Task<List<Cliente>> GetAllPagedAsync(int page, int pageSize) =>
            _ctx.Cliente
                .OrderBy(c => c.nomeCliente)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        public Task<int> CountAsync() =>
            _ctx.Cliente.CountAsync();

        public void Add(Cliente entity) =>
            _ctx.Cliente.Add(entity);
        public void Remove(Cliente entity) =>
            _ctx.Cliente.Remove(entity);
    }
}
