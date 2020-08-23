using HungryPizza.Domain.Entities;
using HungryPizza.Domain.Interfaces.Repositories;
using HungryPizza.Infra.Data.Context;
using HungryPizza.Infra.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HungryPizza.Infra.Data.Repositories
{
    public class RequestPizzaRepository : BaseRepository<RequestPizza>, IRequestPizzaRepository
    {
        public RequestPizzaRepository(SQLContext ctx, IRabbitMQPublisher r) : base(ctx, r)
        {
        }

        #region Validations
        public async Task<bool> IdPizzaExists(int idPizza)
        {
            return await ctx.Pizza.AnyAsync(a => a.Id == idPizza && a.Active);
        }
        #endregion

        #region CRUD
        public async Task<List<RequestPizza>> CreateRequestPizzas(List<RequestPizza> newRequestPizzas)
        {
            await ctx.RequestPizza.AddRangeAsync(newRequestPizzas);
            await ctx.SaveChangesAsync();
            newRequestPizzas.ForEach(e => _rabbitMQPublisher.SendMessage("Create_RequestPizza", e));
            return newRequestPizzas;
        }
        #endregion
    }
}
