using HungryPizza.Infra.Data.MongoDB.Collections;
using HungryPizza.Infra.Data.MongoDB.Interfaces;
using MongoDB.Driver;

namespace HungryPizza.Infra.Data.MongoDB.Context
{
    public class MongoDBContext : IMongoDBContext
    {
        private readonly IMongoDatabase _db;

        public MongoDBContext(string connectionString, string database)
        {
            var client = new MongoClient(connectionString);
            _db = client.GetDatabase(database);
        }

        public IMongoCollection<CustomerCollection> Customers => _db.GetCollection<CustomerCollection>("Customer");
        public IMongoCollection<UserCollection> Users => _db.GetCollection<UserCollection>("User");
        public IMongoCollection<PizzaCollection> Pizzas => _db.GetCollection<PizzaCollection>("Pizza");
        public IMongoCollection<RequestCollection> Requests => _db.GetCollection<RequestCollection>("Request");
        public IMongoCollection<RequestPizzaCollection> RequestPizzas => _db.GetCollection<RequestPizzaCollection>("RequestPizza");
    }
}
