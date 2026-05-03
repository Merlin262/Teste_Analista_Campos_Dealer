using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Domain.Interface
{
    public interface IProdutoRepository
    {
        Task<Produto> GetByIdAsync(Guid id);
        Task<List<Produto>> GetAllPagedAsync(int page, int pageSize);
        Task<int> CountAsync();
        void Add(Produto entity);
        void Remove(Produto entity);
        void AddHistorico(ProdutoPrecoHistorico historico);
    }
}
