using HungryPizza.Domain.Entities;
using HungryPizza.Domain.Interfaces.Repositories;
using HungryPizza.Infra.Data.Context;
using HungryPizza.Infra.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HungryPizza.Infra.Data.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(SQLContext ctx, IRabbitMQPublisher r) : base(ctx, r)
        {
        }

        #region Validations
        public async Task<bool> IdCustomerExists(int idCustomer)
        {
            return await ctx.Customer
                .Join(ctx.User,
                    a => a.IdUser,
                    b => b.Id,
                    (a, b) => new { a, b }
                )
                .AnyAsync(x => x.a.Id == idCustomer && x.b.Active);
        }
        #endregion

        #region CRUD
        public async Task<Customer> CreateCustomer(Customer customer)
        {
            ctx.Customer.Add(customer);
            await ctx.SaveChangesAsync();
            _rabbitMQPublisher.SendMessage("Create_Customer", customer);

            return customer;
        }

        public async Task UpdateCustomer(Customer customer)
        {
            ctx.Customer.Update(customer);
            await ctx.SaveChangesAsync();
            _rabbitMQPublisher.SendMessage("Update_Customer", customer);
        }

        public async Task<Customer> GetCustomer(int idCustomer)
        {
            return await ctx.Customer.AsNoTracking().FirstOrDefaultAsync(f => f.Id == idCustomer);
        }
        #endregion
    }
}
