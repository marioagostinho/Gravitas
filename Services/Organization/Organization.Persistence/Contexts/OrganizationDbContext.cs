using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entities;
using Entity = Organization.Domain.Entities;

namespace Organization.Persistence.Contexts
{
    public class OrganizationDbContext : DbContext
    {
        public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options) : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }

        public DbSet<Entity.Organization> Organization { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrganizationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
