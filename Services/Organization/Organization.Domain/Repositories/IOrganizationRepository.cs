using Entity = Organization.Domain.Entities;

namespace Organization.Domain.Repositories
{
    public interface IOrganizationRepository : IBaseRepository<Entity.Organization>
    {
    }
}
