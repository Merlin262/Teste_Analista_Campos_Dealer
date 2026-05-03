using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Application.Common;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Clientes.Queries.GetAllClientes
{
    public class GetAllClientesQuery : IRequest<PagedResult<Cliente>>
    {
        public int Page { get; }
        public GetAllClientesQuery(int page = 1) { Page = page < 1 ? 1 : page; }
    }
}
