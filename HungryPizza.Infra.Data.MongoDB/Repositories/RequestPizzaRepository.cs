using HungryPizza.Infra.Data.MongoDB.Collections;
using HungryPizza.Infra.Data.MongoDB.Interfaces;
using HungryPizza.Infra.Data.MongoDB.Interfaces.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HungryPizza.Infra.Data.MongoDB.Repositories
{
    public class RequestPizzaRepository : IRequestPizzaRepository
    {
        private readonly IMongoDBContext _db;
        public RequestPizzaRepository(IMongoDBContext db)
        {
            _db = db;
        }
        public async Task Create(RequestPizzaCollection collection)
        {
            await _db.RequestPizzas.InsertOneAsync(collection);
        }

        public async Task<IEnumerable<RequestPizzaCollection>> Get(Expression<Func<RequestPizzaCollection, bool>> expression)
        {
            return await _db
                             .RequestPizzas
                             .Find(expression)
                             .ToListAsync();
        }

        public async Task<bool> Update(RequestPizzaCollection collection)
        {
            ReplaceOneResult updateResult =
                await _db
                        .RequestPizzas
                        .ReplaceOneAsync(
                            filter: g => g.Id == collection.Id,
                            replacement: collection);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
