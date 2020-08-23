using HungryPizza.Domain.Interfaces.Repositories;
using HungryPizza.Infra.Data.Context;
using HungryPizza.Infra.Shared.Interfaces;

namespace HungryPizza.Infra.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly IRabbitMQPublisher _rabbitMQPublisher;

        protected SQLContext ctx;

        public BaseRepository(SQLContext ctx, IRabbitMQPublisher r)
        {
            this.ctx = ctx;
            _rabbitMQPublisher = r;
        }
    }
}
