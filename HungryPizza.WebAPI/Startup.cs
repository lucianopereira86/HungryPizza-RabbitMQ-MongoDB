using System;
using System.IO;
using HungryPizza.Infra.Shared.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using AutoMapper;
using HungryPizza.Domain.Mappers;
using HungryPizza.WebAPI.Mappers;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using HungryPizza.Infra.CrossCutting;

namespace HungryPizza.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region AppSettings
            var appSettings = new AppSettings();
            Configuration.Bind(appSettings);
            services.AddSingleton(appSettings);
            #endregion

            services.AddOptions();
            services.AddControllers();
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression();

            #region  SWAGGER
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HungryPizza Web API", Version = "v1" });
                //c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() { In = ParameterLocation.Header, Description = "Informe o token JWT com Bearer no campo", Name = "Authorization", Type = SecuritySchemeType.ApiKey });
                c.ExampleFilters();

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "HungryPizza.xml");
                c.IncludeXmlComments(filePath);
            });
            #endregion

            #region AutoMapper
            services.AddScoped(m => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CommandToEntityMappingProfile());
                cfg.AddProfile(new CollectionToEntityMappingProfile());
                cfg.AddProfile(new ViewModelToCommandMappingProfile(appSettings));
                cfg.AddProfile(new ViewModelToQueryMappingProfile(appSettings));
            }).CreateMapper());
            #endregion

            NativeInjectorBootStrapper.RegisterServices(services, appSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region  SWAGGER
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HungryPizza API V1");
                c.RoutePrefix = "swagger";
            });
            #endregion
        }
    }
}
