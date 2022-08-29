using System.Linq;
using System.Threading.Tasks;
using EA.Core.Domain.Models;
using EA.Core.Infra.Mappings;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using EA.NetDevPack.Data;
using EA.NetDevPack.Domain;
using EA.NetDevPack.Mediator;
using EA.NetDevPack.Messaging;

namespace EA.Core.Infra.Context
{
    public sealed class SqlCoreContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public SqlCoreContext(DbContextOptions<SqlCoreContext> options, IMediatorHandler mediatorHandler) : base(options)
        {
           _mediatorHandler = mediatorHandler;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public  DbSet<Permissions> Permissions { get; set; }
        public  DbSet<Privileges> Privileges { get; set; }
        public  DbSet<ResAttributes> ResAttributes { get; set; }
        public  DbSet<Resources> Resources { get; set; }
        public  DbSet<Roles> Roles { get; set; }
        public  DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfiguration(new PermissionMap());
            modelBuilder.ApplyConfiguration(new PrivilegeMap());
            modelBuilder.ApplyConfiguration(new ResAttributeMap());
            modelBuilder.ApplyConfiguration(new ResourceMap()); 
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            base.OnModelCreating(modelBuilder);
        }

        public async Task<bool> Commit()
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await  _mediatorHandler.PublishDomainEvents(this).ConfigureAwait(false);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
           
              var success = await SaveChangesAsync() > 0; 
            
            return success;
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublishDomainEvents<T>(this IMediatorHandler mediator, T ctx) where T : DbContext
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
