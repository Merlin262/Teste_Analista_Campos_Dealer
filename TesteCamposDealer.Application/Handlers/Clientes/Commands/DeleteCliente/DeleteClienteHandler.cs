using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Exceptions;
using TesteCamposDealer.Domain.Interface;

namespace TesteCamposDealer.Application.Handlers.Clientes.Commands.DeleteCliente
{
    public class DeleteClienteHandler : IRequestHandler<DeleteClienteCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        public DeleteClienteHandler(IUnitOfWork uow) { _uow = uow; }

        public async Task<bool> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _uow.Clientes.GetByIdAsync(request.IdCliente);
            if (cliente == null)
                throw new NotFoundException("Cliente", request.IdCliente);

            var vendas = await _uow.Vendas.GetByClienteAsync(request.IdCliente);
            if (vendas.Count > 0)
                throw new BusinessRuleException("Cliente possui vendas vinculadas e não pode ser excluído.");

            _uow.Clientes.Remove(cliente);
            await _uow.CommitAsync();
            return true;
        }
    }
}
