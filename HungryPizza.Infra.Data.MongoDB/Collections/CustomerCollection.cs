namespace HungryPizza.Infra.Data.MongoDB.Collections
{
    public class CustomerCollection: BaseCollection
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

    }
}
