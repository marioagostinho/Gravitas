using Identity.Domain.Entities;
using Identity.Domain.Repositories;
using Identity.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly IdentityDbContext _context;

        public BaseRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync(bool track = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (track == false)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, bool track = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!track)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<Guid> CreateAsync(T entity)
        {
            var entry = await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entry.Entity.Id;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            _context.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
