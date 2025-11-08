using BookstoreApplication.Models;

namespace BookstoreApplication.Repositories.Interfaces
{
    public interface IAuthorRepository : IRepository<Author> 
    {
        Task<(List<Author> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    }

}
