using Rookies.Domain;
using Rookies.Domain.Abstractions;
using Rookies.Persistence.Configurations;

namespace Rookies.Persistence
{
    public class RookiesDbContext : DbContext, IUnitOfWork
    {
        public RookiesDbContext(DbContextOptions<RookiesDbContext> options) : base(options)
        { }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public async Task SaveChangesAsync()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditableEntity &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var auditableEntity = (IAuditableEntity)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    auditableEntity.CreatedAt = DateTime.UtcNow;
                    auditableEntity.CreatedBy = Guid.NewGuid();
                }

                auditableEntity.ModifiedAt = DateTime.UtcNow;
                auditableEntity.ModifiedBy = Guid.NewGuid();
            }
            await base.SaveChangesAsync();
        }
    }
}