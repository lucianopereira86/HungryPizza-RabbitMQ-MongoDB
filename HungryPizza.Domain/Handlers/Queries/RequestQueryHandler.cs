using HungryPizza.Infra.Data.MongoDB.Interfaces.Repositories;
using HungryPizza.Domain.Queries.Request;
using HungryPizza.Infra.Shared.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HungryPizza.Infra.Shared.CommandQuery;
using AutoMapper;
using System.Collections.Generic;
using HungryPizza.Domain.Entities;
using HungryPizza.Infra.Data.MongoDB.Collections.History;
using HungryPizza.Domain.Entities.History;

namespace HungryRequest.Domain.Handlers.Queries
{
    public class RequestQueryHandler : CommandQueryHandler,
        IRequestHandler<RequestGetQuery, ICommandQuery>,
        IRequestHandler<RequestHistoryQuery, ICommandQuery>
    {
        private readonly IRequestRepository _repo;

        public RequestQueryHandler(IMapper m, IRequestRepository repo):base(m)
        {
            _repo = repo;
        }

        public async Task<ICommandQuery> Handle(RequestGetQuery query, CancellationToken cancellationToken)
        {
            var collections = await _repo.Get(query.Get());
            if (collections.ToList().Count == 0)
            {
                query.AddError(2011);
            }
            else
            {
                var requests = _mapper.Map<IEnumerable<Request>>(collections);
                query.SetData(requests);
            }
            return query;
        }

        public async Task<ICommandQuery> Handle(RequestHistoryQuery query, CancellationToken cancellationToken)
        {
            // VALIDATE QUERY
            if (!query.IsValid())
                return query;

            // GET REQUEST HISTORY
            var collections = await _repo.GetHistoryByIdCustomer(query.IdCustomer);
            if (collections.ToList().Count == 0)
            {
                query.AddError(2012);
            }
            else
            {
                var requests = _mapper.Map<IEnumerable<RequestHistory>>(collections);
                query.SetData(requests);
            }
            return query;
        }
    }
}
