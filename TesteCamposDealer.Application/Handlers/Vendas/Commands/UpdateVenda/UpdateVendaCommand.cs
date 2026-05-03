using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Dto;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Commands.UpdateVenda
{
    public class UpdateVendaCommand : IRequest<Venda>
    {
        public Guid idVenda { get; set; }
        public List<VendaItemRequest> itens { get; set; }
    }
}
