using System.IO;
using LinqToDB.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using LinqToDB.Data;
using LinqToDB.DataProvider.MySql;
using RawRabbit.Enrichers.GlobalExecutionId;
using RawRabbit.vNext;
using RawRabbit.vNext.Pipe;
using RawRabbit;
using Medico.Service.DynamicForm.Interfaces;
using Medico.Service.DynamicForm.ViewModels;
using System;
using RawRabbit.Configuration;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Enrichers.HttpContext;
using Microsoft.AspNetCore.Http.Extensions;
using System.Linq;
using Medico.Service.DynamicForm.Repository;

namespace Medico.Service.DynamicForm
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            //_configuration = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            this._configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //MySqlDataProvider DataConnectionReadOnly = new MySqlDataProvider();
            //DataConnectionReadOnly.CreateConnection(Configuration["Data:DefaultConnection:ConnectionString"]);

            DataConnection
             .AddConfiguration(
                 "DefaultConnection",
                 _configuration["Data:DefaultConnection:ConnectionString"],
                 new MySqlDataProvider());

            DataConnection
             .AddConfiguration(
                 "ReadOnlyConnection",
                 _configuration["Data:ReadOnlyConnection:ConnectionString"],
                 new MySqlDataProvider());

            DataConnection.DefaultConfiguration = "DefaultConnection";

            //Configurations
            services.Configure<MedicoConfig>(_configuration);

            services.AddTransient<IRepository<TrDynamicFormPatient>, DynamicFormPatientRepository>();
            services.AddTransient<IRepository<MsDynamicFormTemplates>, DynamicFormTemplateRepository>();

            services.AddRawRabbit(new RawRabbitOptions
            {
                ClientConfiguration = GetRawRabbitConfiguration(),
                Plugins = p => p
                    .UseStateMachine()
                    .UseGlobalExecutionId()
                    .UseHttpContext()
                    .UseMessageContext(c =>
                    {
                        return new Medico.Shared.EventBrooker.TodoContext
                        {
                            Source = c.GetHttpContext().Request.GetDisplayUrl(),
                            ExecutionId = c.GetGlobalExecutionId(),
                            SessionId = c.GetHttpContext().Request.Cookies["medicorabbit:sessionid"]
                        };
                    })
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Medico DynamicForm API", Version = "v1" });
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medico DynamicForm API");
            });
            app.UseMvcWithDefaultRoute();
            app.UseMvc();
        }

        private RawRabbitConfiguration GetRawRabbitConfiguration()
        {
            var section = _configuration.GetSection("RawRabbit");
            if (!section.GetChildren().Any())
            {
                throw new ArgumentException($"Unable to configuration section 'RawRabbit'. Make sure it exists in the provided configuration");
            }
            return section.Get<RawRabbitConfiguration>();
        }
    }
}
