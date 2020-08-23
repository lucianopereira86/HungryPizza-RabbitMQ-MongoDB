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
    public class PizzaRepository : IPizzaRepository
    {
        private readonly IMongoDBContext _db;
        public PizzaRepository(IMongoDBContext db)
        {
            _db = db;
        }
        public async Task Create(PizzaCollection collection)
        {
            await _db.Pizzas.InsertOneAsync(collection);
        }

        public async Task<IEnumerable<PizzaCollection>> Get(Expression<Func<PizzaCollection, bool>> expression)
        {
            return await _db
                        .Pizzas
                        .Find(expression)
                        .ToListAsync();
        }

        public async Task<bool> Update(PizzaCollection collection)
        {
            ReplaceOneResult updateResult =
                await _db
                        .Pizzas
                        .ReplaceOneAsync(
                            filter: g => g.Id == collection.Id,
                            replacement: collection);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task Delete()
        {
            await _db.Pizzas.DeleteManyAsync(x => x.Active);
        }
    }
}
