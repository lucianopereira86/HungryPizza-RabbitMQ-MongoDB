namespace HungryPizza.Infra.Data.MongoDB.Collections.History
{
    public class RequestHistoryPizzaCollection
    {
        public RequestHistoryPizzaCollection(RequestHistoryPizzaInfoCollection firstHalfPizza, RequestHistoryPizzaInfoCollection secondHalfPizza, int quantity, decimal total)
        {
            FirstHalfPizza = firstHalfPizza;
            SecondHalfPizza = secondHalfPizza;
            Quantity = quantity;
            Total = total;
        }

        public RequestHistoryPizzaInfoCollection FirstHalfPizza { get; private set; }
        public RequestHistoryPizzaInfoCollection SecondHalfPizza { get; private set; }
        public int Quantity { get; private set; }
        public decimal Total { get; private set; }
    }
}
