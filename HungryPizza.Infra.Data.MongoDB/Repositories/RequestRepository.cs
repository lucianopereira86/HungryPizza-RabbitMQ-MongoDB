using HungryPizza.Infra.Data.MongoDB.Collections;
using HungryPizza.Infra.Data.MongoDB.Collections.History;
using HungryPizza.Infra.Data.MongoDB.Interfaces;
using HungryPizza.Infra.Data.MongoDB.Interfaces.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HungryPizza.Infra.Data.MongoDB.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly IMongoDBContext _db;
        private readonly IPizzaRepository _pizzaRepository;
        private readonly IRequestPizzaRepository _requestPizzaRepository;
        public RequestRepository(IMongoDBContext db, IPizzaRepository p, IRequestPizzaRepository rp)
        {
            _db = db;
            _pizzaRepository = p;
            _requestPizzaRepository = rp;
        }
        public async Task Create(RequestCollection collection)
        {
            await _db.Requests.InsertOneAsync(collection);
        }

        public async Task<IEnumerable<RequestCollection>> Get(Expression<Func<RequestCollection, bool>> expression)
        {
            return await _db
                        .Requests
                        .Find(expression)
                        .ToListAsync();
        }

        public async Task<IEnumerable<RequestHistoryCollection>> GetHistoryByIdCustomer(int idCustomer)
        {
            var requestHistory = new List<RequestHistoryCollection>();
            var requests = await Get(x => x.IdCustomer == idCustomer);
            if (requests.Count() == 0)
                return requestHistory;

            var query = (from r in requests.AsQueryable()
                        join c in _db.Customers.AsQueryable() on r.IdCustomer equals c.Id into jrc
                        from jrcx in jrc.DefaultIfEmpty()
                        where jrcx.Id == idCustomer
                        select new
                        {
                            r,
                            c = jrcx
                        })
                        .ToList()
                        .OrderBy(o => o.r.Id)
                        .ToList();
            query.ForEach(e => { e.r.SetAddress(e.c.Address); });

            var pizzas = _pizzaRepository.Get(x => x.Active).Result;
            
            query
                .Select(s => s.r)
                .ToList()
                .ForEach(e =>
                {
                    var requestHistoryPizzas = new List<RequestHistoryPizzaCollection>();
                    var requestPizzas = _requestPizzaRepository.Get(w => w.IdRequest == e.Id).Result.OrderBy(o => o.Id).ToList();
                    requestPizzas.ForEach(e =>
                    {
                        var firstHalfPizza = pizzas.FirstOrDefault(w => w.Id == e.IdPizzaFirstHalf);
                        var secondHalfPizza = pizzas.FirstOrDefault(w => w.Id == e.IdPizzaSecondHalf);

                        var requestHistoryPizza = new RequestHistoryPizzaCollection(
                            new RequestHistoryPizzaInfoCollection(firstHalfPizza.Name, firstHalfPizza.Price),
                            new RequestHistoryPizzaInfoCollection(secondHalfPizza.Name, secondHalfPizza.Price),
                            e.Quantity,
                            e.Total);
                        requestHistoryPizzas.Add(requestHistoryPizza);
                    });
                    requestHistory.Add(new RequestHistoryCollection(e.Uid, e.CreatedAt, requestHistoryPizzas, e.Quantity, e.Total));
                });
            return requestHistory;
        }

        public async Task<bool> Update(RequestCollection collection)
        {
            ReplaceOneResult updateResult =
                await _db
                        .Requests
                        .ReplaceOneAsync(
                            filter: g => g.Id == collection.Id,
                            replacement: collection);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
