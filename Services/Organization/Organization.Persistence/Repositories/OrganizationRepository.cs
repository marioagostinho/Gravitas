using Organization.Domain.Repositories;
using Organization.Persistence.Contexts;
using Entity = Organization.Domain.Entities;

namespace Organization.Persistence.Repositories
{
    public class OrganizationRepository : BaseRepository<Entity.Organization>, IOrganizationRepository
    {
        public OrganizationRepository(OrganizationDbContext context) : base(context)
        {
        }
    }
}
