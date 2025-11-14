using BookstoreApplication.Models.Entities;

namespace BookstoreApplication.Models.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<List<Book>> GetAllWithIncludesAsync(bool asNoTracking = true);
        Task<Book?> GetOneWithIncludesAsync(int id, bool asNoTracking = true);
        IQueryable<Book> QueryWithIncludes(bool asNoTracking = true);
    }
}
