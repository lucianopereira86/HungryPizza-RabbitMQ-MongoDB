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
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDBContext _db;
        public UserRepository(IMongoDBContext db)
        {
            _db = db;
        }
        public async Task Create(UserCollection collection)
        {
            await _db.Users.InsertOneAsync(collection);
        }

        public async Task<IEnumerable<UserCollection>> Get(Expression<Func<UserCollection, bool>> expression)
        {
            return await _db
                             .Users
                             .Find(expression)
                             .ToListAsync();
        }

        public async Task<bool> Update(UserCollection collection)
        {
            ReplaceOneResult updateResult =
                await _db
                        .Users
                        .ReplaceOneAsync(
                            filter: g => g.Id == collection.Id,
                            replacement: collection);
            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
