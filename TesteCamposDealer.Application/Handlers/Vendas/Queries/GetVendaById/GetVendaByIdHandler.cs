using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendaById
{
    public class GetVendaByIdHandler : IRequestHandler<GetVendaByIdQuery, Venda>
    {
        private readonly IUnitOfWork _uow;
        public GetVendaByIdHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<Venda> Handle(GetVendaByIdQuery request, CancellationToken cancellationToken)
        {
            var venda = await _uow.Vendas.GetByIdWithDetailsAsync(request.IdVenda);
            if (venda == null)
                throw new NotFoundException("Venda", request.IdVenda);
            return venda;
        }
    }
}
