using HungryPizza.Infra.Data.MongoDB.Collections;
using System.Threading.Tasks;

namespace HungryPizza.Infra.Data.MongoDB.Interfaces.Repositories
{
    public interface IPizzaRepository : IBaseRepository<PizzaCollection>
    {
        Task Delete();
    }
}
