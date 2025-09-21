using BookstoreApplication.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BookstoreApplication.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(bool asNoTracking = true);
        Task<T?> GetByIdAsync(int id, bool asNoTracking = true);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> SaveChangesAsync();
    }
}
