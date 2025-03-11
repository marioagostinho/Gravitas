using Identity.Domain.Entities;

namespace Identity.Domain.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<bool> ExistsByEmailAsync(string email);
        public Task<User?> GetByEmailAsync(string email, bool track = false);
    }
}
