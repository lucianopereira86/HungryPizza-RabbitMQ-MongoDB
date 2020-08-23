namespace HungryPizza.Infra.Shared.Interfaces
{
    public interface IRabbitMQPublisher
    {
        void SendMessage(string queueName, object obj);
    }
}
