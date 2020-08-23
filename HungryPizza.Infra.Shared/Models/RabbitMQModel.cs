namespace HungryPizza.Infra.Shared.Models
{
    public class RabbitMQModel
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string[] Queues { get; set; }
    }
}
