using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetVendaById
{
    public class GetVendaByIdQuery : IRequest<Venda>
    {
        public Guid IdVenda { get; }
        public GetVendaByIdQuery(Guid idVenda) { IdVenda = idVenda; }
    }
}
