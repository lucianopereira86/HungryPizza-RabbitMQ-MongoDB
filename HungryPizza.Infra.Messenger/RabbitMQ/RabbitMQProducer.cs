using HungryPizza.Infra.Shared.Interfaces;
using HungryPizza.Infra.Shared.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace HungryPizza.Infra.Messenger.RabbitMQ
{
    public class RabbitMQProducer: IRabbitMQPublisher
    {
        private readonly AppSettings _appSettings;
        public RabbitMQProducer(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }
        public void SendMessage(string queueName, object obj)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
            var rabbitMQ = _appSettings.ConnectionStrings.RabbitMQ;
            var factory = new ConnectionFactory() { HostName = rabbitMQ.HostName, UserName = rabbitMQ.UserName, Password = rabbitMQ.Password };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }
}
