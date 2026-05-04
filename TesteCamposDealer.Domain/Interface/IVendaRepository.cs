using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Domain.Interface
{
    public interface IVendaRepository
    {
        Task<Venda> GetByIdAsync(Guid id);
        Task<Venda> GetByIdWithDetailsAsync(Guid id);
        Task<Venda> GetByIdWithItensAsync(Guid id);
        Task<List<Venda>> GetAllWithDetailsPagedAsync(int page, int pageSize);
        Task<int> CountAsync();
        Task<List<Venda>> GetByClienteAsync(Guid idCliente);
        Task<List<Venda>> GetByClientePagedAsync(Guid idCliente, int page, int pageSize);
        Task<int> CountByClienteAsync(Guid idCliente);
        Task<List<Venda>> GetRankingAsync();
        Task<List<Venda>> GetRankingPagedAsync(int page, int pageSize);
        Task<bool> HasItensByProdutoAsync(Guid idProduto);
        void Add(Venda entity);
        void Remove(Venda entity);
        void RemoveItens(IEnumerable<VendaItem> itens);
    }
}
