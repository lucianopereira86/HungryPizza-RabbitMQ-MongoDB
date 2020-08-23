using HungryPizza.Infra.Data.MongoDB.Collections;
using HungryPizza.Infra.Data.MongoDB.Interfaces.Repositories;
using HungryPizza.Infra.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HungryPizza.Infra.Messenger.RabbitMQ
{
    public class RabbitMQConsumer : BackgroundService, IHostedService
    {
        private IConnection _connection;
        private IUserRepository _userRepo;
        private ICustomerRepository _customerRepo;
        private IPizzaRepository _pizzaRepo;
        private IRequestRepository _requestRepo;
        private IRequestPizzaRepository _requestPizzaRepo;

        public IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                #region RabbitMQ
                var appSettings = scope.ServiceProvider.GetRequiredService<AppSettings>();

                var rabbitMQ = appSettings.ConnectionStrings.RabbitMQ;

                var factory = new ConnectionFactory() { 
                    HostName = rabbitMQ.HostName, UserName = rabbitMQ.UserName, Password = rabbitMQ.Password 
                };
                _connection = factory.CreateConnection();
                _connection.ConnectionShutdown += _connection_ConnectionShutdown;
                #endregion

                #region MongoDB
                _userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                _customerRepo = scope.ServiceProvider.GetRequiredService<ICustomerRepository>();
                _pizzaRepo = scope.ServiceProvider.GetRequiredService<IPizzaRepository>();
                _requestRepo = scope.ServiceProvider.GetRequiredService<IRequestRepository>();
                _requestPizzaRepo = scope.ServiceProvider.GetRequiredService<IRequestPizzaRepository>();

                PreemptiveInsertsMongoDB(scope);
                #endregion

                stoppingToken.ThrowIfCancellationRequested();

                rabbitMQ.Queues.ToList().ForEach(e => CreateChannel(e));
            }

            return Task.CompletedTask;
        }

        private void PreemptiveInsertsMongoDB(IServiceScope scope)
        {
            Task.Run(() => InsertPizzas(scope)).Wait();
        }

        private async Task InsertPizzas(IServiceScope scope)
        {
            var mysqlPizzaRepo = scope.ServiceProvider.GetRequiredService<Domain.Interfaces.Repositories.IPizzaRepository>();
            var pizzas = await mysqlPizzaRepo.Get(x => x.Active);
            await _pizzaRepo.Delete();
            pizzas.ToList().ForEach(async(e) => await _pizzaRepo.Create(new PizzaCollection(e.Id, e.Name, e.Price, e.Active)));
        }

        private void CreateChannel(string queueName)
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                HandleMessage(queueName, content);

                channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += Consumer_Shutdown;
            consumer.Registered += Consumer_Registered;
            consumer.Unregistered += Consumer_Unregistered;
            consumer.ConsumerCancelled += Consumer_ConsumerCancelled;

            channel.BasicConsume(queueName, false, consumer);
        }

        private void HandleMessage(string queueName, string content)
        {
            string[] split = queueName.Split("_");
            string action = split[0];
            string entity = split[1];

            switch (action)
            {
                case "Create":
                    CreateCollection(entity, content);
                    break;
                case "Update":
                    UpdateCollection(entity, content);
                    break;
                default:
                    break;
            }
        }

        private void CreateCollection(string entity, string content)
        {

            switch (entity)
            {
                case "User":
                    _userRepo.Create(JsonConvert.DeserializeObject<UserCollection>(content));
                    break;
                case "Customer":
                    _customerRepo.Create(JsonConvert.DeserializeObject<CustomerCollection>(content));
                    break;
                case "Pizza":
                    _pizzaRepo.Create(JsonConvert.DeserializeObject<PizzaCollection>(content));
                    break;
                case "Request":
                    _requestRepo.Create(JsonConvert.DeserializeObject<RequestCollection>(content));
                    break;
                case "RequestPizza":
                    _requestPizzaRepo.Create(JsonConvert.DeserializeObject<RequestPizzaCollection>(content));
                    break;
                default:
                    break;
            }
        }

        private void UpdateCollection(string entity, string content)
        {

            switch (entity)
            {
                case "User":
                    _userRepo.Update(JsonConvert.DeserializeObject<UserCollection>(content));
                    break;
                case "Customer":
                    _customerRepo.Update(JsonConvert.DeserializeObject<CustomerCollection>(content));
                    break;
                case "Pizza":
                    _pizzaRepo.Update(JsonConvert.DeserializeObject<PizzaCollection>(content));
                    break;
                case "Request":
                    _requestRepo.Update(JsonConvert.DeserializeObject<RequestCollection>(content));
                    break;
                case "RequestPizza":
                    _requestPizzaRepo.Update(JsonConvert.DeserializeObject<RequestPizzaCollection>(content));
                    break;
                default:
                    break;
            }
        }

        private void Consumer_ConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Consumer_Unregistered(object sender, ConsumerEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Consumer_Registered(object sender, ConsumerEventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        private void Consumer_Shutdown(object sender, ShutdownEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
