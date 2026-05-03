using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Domain.Interface;

namespace TesteCamposDealer.Application.Handlers.Vendas.Commands.DeleteVenda
{
    public class DeleteVendaHandler : IRequestHandler<DeleteVendaCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        public DeleteVendaHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<bool> Handle(DeleteVendaCommand request, CancellationToken cancellationToken)
        {
            var venda = await _uow.Vendas.GetByIdAsync(request.IdVenda);
            if (venda == null)
                throw new NotFoundException("Venda", request.IdVenda);

            _uow.Vendas.Remove(venda);
            await _uow.CommitAsync();
            return true;
        }
    }
}
