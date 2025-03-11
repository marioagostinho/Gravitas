using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using Identity.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IdentityDbContext context) : base(context)
        { 
        }

        public Task<bool> ExistsByEmailAsync(string email)
        {
            return _context.Set<User>().AnyAsync(e => e.Email == email);
        }

        public async Task<User?> GetByEmailAsync(string email, bool track = false)
        {
            IQueryable<User> query = _context.Set<User>();

            if (!track)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(e => e.Email == email);
        }

        public override async Task<bool> UpdateAsync(User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            // Block changes in the follow fields
            _context.Entry(entity).Property(p => p.Email).IsModified = false;
            _context.Entry(entity).Property(p => p.PasswordHash).IsModified = false;
            _context.Entry(entity).Property(p => p.PasswordSalt).IsModified = false;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
