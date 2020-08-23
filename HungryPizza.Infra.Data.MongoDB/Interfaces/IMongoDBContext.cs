using HungryPizza.Infra.Data.MongoDB.Collections;
using MongoDB.Driver;

namespace HungryPizza.Infra.Data.MongoDB.Interfaces
{
    public interface IMongoDBContext
    {
        IMongoCollection<CustomerCollection> Customers { get; }
        IMongoCollection<UserCollection> Users { get; }
        IMongoCollection<PizzaCollection> Pizzas { get; }
        IMongoCollection<RequestCollection> Requests { get; }
        IMongoCollection<RequestPizzaCollection> RequestPizzas { get; }
    }
}
