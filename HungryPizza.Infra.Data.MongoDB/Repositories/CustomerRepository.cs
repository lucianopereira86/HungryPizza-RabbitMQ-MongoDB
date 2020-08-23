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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IMongoDBContext _db;
        public CustomerRepository(IMongoDBContext db)
        {
            _db = db;
        }
        public async Task Create(CustomerCollection collection)
        {
            await _db.Customers.InsertOneAsync(collection);
        }

        public async Task<IEnumerable<CustomerCollection>> Get(Expression<Func<CustomerCollection, bool>> expression)
        {
            return await _db
                             .Customers
                             .Find(expression)
                             .ToListAsync();
        }

        public async Task<bool> Update(CustomerCollection collection)
        {
            ReplaceOneResult updateResult =
                await _db
                        .Customers
                        .ReplaceOneAsync(
                            filter: g => g.Id == collection.Id,
                            replacement: collection);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
