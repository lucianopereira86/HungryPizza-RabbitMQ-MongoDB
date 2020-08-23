namespace HungryPizza.Infra.Data.MongoDB.Collections
{
    public class PizzaCollection: BaseCollection
    {
        public PizzaCollection(int id, string name, decimal price, bool active)
        {
            Id = id;
            Name = name;
            Price = price;
            Active = active;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }

    }
}
