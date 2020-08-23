namespace HungryPizza.Infra.Shared.Models
{
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
        public RabbitMQModel RabbitMQ { get; set; }
        public MongoDBModel MongoDB { get; set; }
    }
}
