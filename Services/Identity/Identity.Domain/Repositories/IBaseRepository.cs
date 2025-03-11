using Identity.Domain.Entities;

namespace Identity.Domain.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        public Task<bool> ExistsAsync(Guid id);
        public Task<IReadOnlyList<T>> GetAllAsync(bool track = false);
        public Task<T?> GetByIdAsync(Guid id, bool track = false);
        public Task<Guid> CreateAsync(T entity);
        public Task<bool> UpdateAsync(T entity);
        public Task<bool> DeleteAsync(T entity);
    }
}
