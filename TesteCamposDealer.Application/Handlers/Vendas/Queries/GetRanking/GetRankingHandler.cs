using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TesteCamposDealer.Domain.Interface;
using TesteCamposDealer.Models;

namespace TesteCamposDealer.Application.Handlers.Vendas.Queries.GetRanking
{
    public class GetRankingHandler : IRequestHandler<GetRankingQuery, List<Venda>>
    {
        private readonly IUnitOfWork _uow;
        public GetRankingHandler(IUnitOfWork uow) { _uow = uow; }

        public Task<List<Venda>> Handle(GetRankingQuery request, CancellationToken cancellationToken)
            => _uow.Vendas.GetRankingAsync();
    }
}
