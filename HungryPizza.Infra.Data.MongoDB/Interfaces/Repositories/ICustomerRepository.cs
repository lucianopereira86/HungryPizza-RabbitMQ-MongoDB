using HungryPizza.Infra.Data.MongoDB.Collections;

namespace HungryPizza.Infra.Data.MongoDB.Interfaces.Repositories
{
    public interface ICustomerRepository : IBaseRepository<CustomerCollection>
    {
    }
}
