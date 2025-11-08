using BookstoreApplication.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _db;
        protected readonly DbSet<T> _set;

        public Repository(AppDbContext db)
        {
            _db = db;
            _set = _db.Set<T>();
        }

        public virtual async Task<List<T>> GetAllAsync(bool asNoTracking = true)
            => asNoTracking ? await _set.AsNoTracking().ToListAsync()
                            : await _set.ToListAsync();

        public virtual async Task<T?> GetByIdAsync(int id, bool asNoTracking = true)
        {
            if (asNoTracking)
                return await _set.AsNoTracking()
                                 .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            return await _set.FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _set.AddAsync(entity);
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _set.FindAsync(id);
            if (entity is not null) _set.Remove(entity);
        }

        public Task<bool> ExistsAsync(int id)
            => _set.AnyAsync(e => EF.Property<int>(e, "Id") == id);

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
