using HungryPizza.Infra.Data.MongoDB.Interfaces.Repositories;
using HungryPizza.Domain.Queries.Pizza;
using HungryPizza.Infra.Shared.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HungryPizza.Infra.Shared.CommandQuery;
using AutoMapper;
using HungryPizza.Domain.Entities;
using System.Collections.Generic;

namespace HungryPizza.Domain.Handlers.Queries
{
    public class PizzaQueryHandler: CommandQueryHandler,
        IRequestHandler<PizzaGetQuery, ICommandQuery>
    {
        private readonly IPizzaRepository _repo;

        public PizzaQueryHandler(IMapper m, IPizzaRepository repo):base(m)
        {
            _repo = repo;
        }

        public async Task<ICommandQuery> Handle(PizzaGetQuery query, CancellationToken cancellationToken)
        {
            var collections = await _repo.Get(query.Get());
            if (collections.ToList().Count == 0)
            {
                query.AddError(2010);
            }
            else
            {
                var pizzas = _mapper.Map<IEnumerable<Pizza>>(collections);
                query.SetData(pizzas);
            }
            return query;
        }
    }
}
