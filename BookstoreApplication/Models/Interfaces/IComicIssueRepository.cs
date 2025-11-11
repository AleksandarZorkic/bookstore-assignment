using BookstoreApplication.Models.Entities;

namespace BookstoreApplication.Models.Interfaces
{
    public interface IComicIssueRepository : IRepository<ComicIssue>
    {
        Task<bool> ExistsByExternalIdAsync(long externalId);
    }
}
