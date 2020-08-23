using HungryPizza.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HungryPizza.Domain.Interfaces.Repositories
{
    public interface IRequestPizzaRepository : IBaseRepository<RequestPizza>
    {
        #region Validations
        Task<bool> IdPizzaExists(int idPizza);
        #endregion

        #region CRUD
        Task<List<RequestPizza>> CreateRequestPizzas(List<RequestPizza> newRequestPizzas); 
        #endregion
    }
}
