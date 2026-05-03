using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Domain.Interface
{
    public interface IClienteRepository
    {
        Task<Cliente> GetByIdAsync(Guid id);
        Task<List<Cliente>> GetAllPagedAsync(int page, int pageSize);
        Task<int> CountAsync();
        void Add(Cliente entity);
        void Remove(Cliente entity);
    }
}
