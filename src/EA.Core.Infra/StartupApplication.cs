using EA.Core.Domain.Interfaces;
using EA.Core.Infra.Context;
using EA.Core.Infra.EventSourcing;
using EA.Core.Infra.Repository;
using EA.Core.Infra.Repository.EventSourcing;
using EA.NetDevPack.Events;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EA.NetDevPack;
using EA.NetDevPack.Mediator;
using System.Reflection;
using MediatR;
using EA.Core.Infra.Consul;
using Microsoft.Extensions.Hosting;
using EA.NetDevPack.Configuration;
using Microsoft.AspNetCore.Http;
using EA.NetDevPack.Context;

namespace EA.Core.Infra
{
    public  class StartupApplication: IStartupApplication
    {
        public int Priority => 2;
        public bool BeforeConfigure => true;

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddConsul(configuration);
            //services.AddDbContext<SqlCoreContext>(options =>options.UseSqlServer(configuration.GetConnectionString("CoreConnection")));
            var connectionStringPlaceHolder = configuration.GetConnectionString("CoreConnection");
            services.AddDbContext<SqlCoreContext>((serviceProvider, dbContextBuilder) =>
            {
                var context = serviceProvider.GetRequiredService<IContextUser>();
               var connectionString = connectionStringPlaceHolder.Replace("{tenant}", context.UserClaims.Tenant).Replace("{zone}", context.UserClaims.Data_Zone);
                dbContextBuilder.UseSqlServer(connectionString);
            });

            //services.AddDbContext<EventStoreSqlContext>(options => options.UseSqlServer(configuration.GetConnectionString("EventConnection"))); 
            var connectionEventPlaceHolder = configuration.GetConnectionString("EventConnection");
            services.AddDbContext<EventStoreSqlContext>((serviceProvider, dbContextBuilder) =>
            {
                var context = serviceProvider.GetRequiredService<IContextUser>();
                var connectionString = connectionEventPlaceHolder.Replace("{tenant}", context.UserClaims.Tenant).Replace("{zone}", context.UserClaims.Data_Zone);
                dbContextBuilder.UseSqlServer(connectionString);
            });

            services.AddScoped<IMediatorHandler, EA.Core.Infra.Bus.MediatorHandler>();
            services.AddScoped<IEventStoreRepository, EventStoreSqlRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddScoped<EventStoreSqlContext>(); 
            //// Infra - Data
            services.AddScoped<IPrivilegeRepository, PrivilegeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<SqlCoreContext>();

            services.AddRabbitMQ(configuration);
        }

        public void Configure(WebApplication application, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                application.UseConsul(application.Lifetime);
            }
            catch (Exception)
            {
            }
        }
    }
}