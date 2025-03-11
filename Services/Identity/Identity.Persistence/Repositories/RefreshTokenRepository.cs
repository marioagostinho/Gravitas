using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using Identity.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IdentityDbContext context) : base(context)
        {
        }

        public async Task<RefreshToken?> GetValidTokenByUserAsync(Guid id, string token, bool track = false)
        {
            IQueryable<RefreshToken> query = _context.Set<RefreshToken>();

            if (!track)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(e => e.UserId == id 
                && e.Token == token 
                && e.ExpiresAt > DateTime.UtcNow 
                && e.IsRevoked == false);
        }
    }
}
