using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Organization.Persistence.Contexts
{
    public class OrganizationDbContextFactory : IDesignTimeDbContextFactory<OrganizationDbContext>
    {
        public OrganizationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrganizationDbContext>();

            // Replace with your actual connection string
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Organization;Trusted_Connection=True;MultipleActiveResultSets=true;");

            return new OrganizationDbContext(optionsBuilder.Options);
        }
    }
}
