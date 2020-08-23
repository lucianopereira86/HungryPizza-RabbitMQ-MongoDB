namespace HungryPizza.Infra.Data.MongoDB.Collections
{
    public class RequestPizzaCollection : BaseCollection
    {
        public int IdRequest { get; set; }
        public int IdPizzaFirstHalf { get; set; }
        public int IdPizzaSecondHalf { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public bool Active { get; set; }
    }
}
