using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendasByCliente
{
    public class GetVendasByClienteHandler : IRequestHandler<GetVendasByClienteQuery, List<Venda>>
    {
        private readonly IUnitOfWork _uow;
        public GetVendasByClienteHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<List<Venda>> Handle(GetVendasByClienteQuery request, CancellationToken cancellationToken)
        {
            var cliente = await _uow.Clientes.GetByIdAsync(request.IdCliente);
            if (cliente == null)
                return null;

            return await _uow.Vendas.GetByClienteAsync(request.IdCliente);
        }
    }
}
