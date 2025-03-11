using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Persistence.Contexts
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();

            // Replace with your actual connection string
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Identity;Trusted_Connection=True;MultipleActiveResultSets=true;");

            return new IdentityDbContext(optionsBuilder.Options);
        }
    }
}
