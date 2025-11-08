using BookstoreApplication.Models;

namespace BookstoreApplication.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<List<Book>> GetAllWithIncludesAsync(bool asNoTracking = true);
        Task<Book?> GetOneWithIncludesAsync(int id, bool asNoTracking = true);

        IQueryable<Book> QueryWithIncludes(bool asNoTracking = true);
    }
}
