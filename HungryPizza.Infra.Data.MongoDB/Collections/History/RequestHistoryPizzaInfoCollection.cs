namespace HungryPizza.Infra.Data.MongoDB.Collections.History
{
    public class RequestHistoryPizzaInfoCollection
    {
        public RequestHistoryPizzaInfoCollection(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public string Name { get; private set; }
        public decimal Price { get; private set; }
    }
}
