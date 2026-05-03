using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCamposDealer.Application.Handlers.Vendas.Commands.DeleteVenda
{
    public class DeleteVendaCommand : IRequest<bool>
    {
        public Guid IdVenda { get; }
        public DeleteVendaCommand(Guid idVenda) { IdVenda = idVenda; }
    }
}
