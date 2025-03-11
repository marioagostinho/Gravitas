using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Organization.Domain.Repositories;
using Organization.Persistence.Contexts;
using Organization.Persistence.Repositories;

namespace Organization.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrganizationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("OrganizationConnectionString"));
            });

            services.AddScoped<IOrganizationRepository, OrganizationRepository>();

            return services;
        }
    }
}
