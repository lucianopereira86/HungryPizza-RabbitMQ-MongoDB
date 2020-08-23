using HungryPizza.Infra.Data.MongoDB.Collections;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HungryPizza.Infra.Data.MongoDB.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T: BaseCollection
    {
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> expression);
        Task Create(T collection);
        Task<bool> Update(T collection);
    }
}
