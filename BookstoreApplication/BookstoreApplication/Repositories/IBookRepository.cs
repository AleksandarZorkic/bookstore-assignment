using BookstoreApplication.Models;

namespace BookstoreApplication.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<List<Book>> GetAllWithIncludesAsync(bool asNoTracking = true);
        Task<Book?> GetOneWithIncludesAsync(int id, bool asNoTracking = true);
    }
}
