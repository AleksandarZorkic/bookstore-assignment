using BookstoreApplication.Models.Entities;

namespace BookstoreApplication.Models.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<(List<Author> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    }

}
