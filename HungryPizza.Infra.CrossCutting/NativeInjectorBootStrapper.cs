using HungryPizza.Domain.Interfaces.Repositories;
using HungryPizza.Infra.Data.Context;
using HungryPizza.Infra.Data.MongoDB.Context;
using HungryPizza.Infra.Data.MongoDB.Interfaces;
using HungryPizza.Infra.Data.Repositories;
using HungryPizza.Infra.Messenger.RabbitMQ;
using HungryPizza.Infra.Shared.Interfaces;
using HungryPizza.Infra.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Reflection;

namespace HungryPizza.Infra.CrossCutting
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services, AppSettings appSettings)
        {

            #region ConnectionStrings
            services.AddDbContextPool<SQLContext>(options => options
                .UseMySql(appSettings.ConnectionStrings.DefaultConnection, mySqlOptions => mySqlOptions
                    .ServerVersion(new Version(1, 0, 0), ServerType.MySql)
            ));
            var mongoDBModel = appSettings.ConnectionStrings.MongoDB;
            services.AddScoped<IMongoDBContext>(x => new MongoDBContext(mongoDBModel.ConnectionString, mongoDBModel.Database));
            #endregion

            #region Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IPizzaRepository, PizzaRepository>();
            services.AddScoped<IRequestPizzaRepository, RequestPizzaRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            #region MongoDB
            services.AddScoped<Data.MongoDB.Interfaces.Repositories.IUserRepository, Data.MongoDB.Repositories.UserRepository>();
            services.AddScoped<Data.MongoDB.Interfaces.Repositories.ICustomerRepository, Data.MongoDB.Repositories.CustomerRepository>();
            services.AddScoped<Data.MongoDB.Interfaces.Repositories.IPizzaRepository, Data.MongoDB.Repositories.PizzaRepository>();
            services.AddScoped<Data.MongoDB.Interfaces.Repositories.IRequestRepository, Data.MongoDB.Repositories.RequestRepository>();
            services.AddScoped<Data.MongoDB.Interfaces.Repositories.IRequestPizzaRepository, Data.MongoDB.Repositories.RequestPizzaRepository>();
            #endregion
            #endregion

            #region MediatR
            services.AddMediatR(
                Assembly.Load("HungryPizza.WebAPI"),
                Assembly.Load("HungryPizza.Domain")
            );
            #endregion

            #region RabbitMQ
            services.AddTransient<IRabbitMQPublisher, RabbitMQProducer>();
            services.AddHostedService<RabbitMQConsumer>();
            #endregion

        }
    }
}
