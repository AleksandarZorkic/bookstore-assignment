using BookstoreApplication.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _db;
        public Repository(AppDbContext db) => _db = db;

        public Task<List<T>> GetAllAsync(bool asNoTracking = true)
            => (asNoTracking ? _db.Set<T>().AsNoTracking() : _db.Set<T>()).ToListAsync();

        public Task<T?> GetByIdAsync(int id, bool asNoTracking = true)
            => (asNoTracking ? _db.Set<T>().AsNoTracking() : _db.Set<T>())
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

        public async Task<T> AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(T entity)
        {
            _db.Set<T>().Update(entity);   
            return Task.CompletedTask;   
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id, asNoTracking: false);
            if (entity != null) _db.Set<T>().Remove(entity);
        }

        public Task<bool> ExistsAsync(int id)
            => _db.Set<T>().AnyAsync(e => EF.Property<int>(e, "Id") == id);

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
