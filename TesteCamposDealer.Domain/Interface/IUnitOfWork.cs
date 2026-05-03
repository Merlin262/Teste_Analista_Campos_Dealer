using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCamposDealer.Domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository Clientes { get; }
        IProdutoRepository Produtos { get; }
        IVendaRepository Vendas { get; }
        Task<int> CommitAsync();
    }
}
