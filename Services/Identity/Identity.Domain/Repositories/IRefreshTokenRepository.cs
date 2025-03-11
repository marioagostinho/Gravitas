using Identity.Domain.Entities;

namespace Identity.Domain.Repositories
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        public Task<RefreshToken?> GetValidTokenByUserAsync(Guid id, string token, bool track = false);
    }
}
