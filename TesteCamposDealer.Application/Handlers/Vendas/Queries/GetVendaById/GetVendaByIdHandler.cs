using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendaById
{
    public class GetVendaByIdHandler : IRequestHandler<GetVendaByIdQuery, Venda>
    {
        private readonly IUnitOfWork _uow;
        public GetVendaByIdHandler(IUnitOfWork uow) { _uow = uow; }

        public Task<Venda> Handle(GetVendaByIdQuery request, CancellationToken cancellationToken)
            => _uow.Vendas.GetByIdWithDetailsAsync(request.IdVenda);
    }
}
