using HungryPizza.Infra.Data.MongoDB.Collections;
using HungryPizza.Infra.Data.MongoDB.Collections.History;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HungryPizza.Infra.Data.MongoDB.Interfaces.Repositories
{
    public interface IRequestRepository : IBaseRepository<RequestCollection>
    {
        Task<IEnumerable<RequestHistoryCollection>> GetHistoryByIdCustomer(int idCustomer);
    }
}
