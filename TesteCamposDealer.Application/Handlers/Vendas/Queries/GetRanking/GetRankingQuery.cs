using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetRanking
{
    public class GetRankingQuery : IRequest<List<Venda>> { }
}
