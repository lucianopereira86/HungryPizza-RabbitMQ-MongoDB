using System;
using System.Collections.Generic;

namespace HungryPizza.Infra.Data.MongoDB.Collections.History
{
    public class RequestHistoryCollection
    {
        public RequestHistoryCollection(Guid uid, DateTime createdAt, List<RequestHistoryPizzaCollection> pizzas, int quantity, decimal total)
        {
            Uid = uid;
            CreatedAt = createdAt;
            Pizzas = pizzas;
            Quantity = quantity;
            Total = total;
        }

        public Guid Uid { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public List<RequestHistoryPizzaCollection> Pizzas { get; private set; }
        public int Quantity { get; private set; }
        public decimal Total { get; private set; }
    }
}
