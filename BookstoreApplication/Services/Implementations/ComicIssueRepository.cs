using BookstoreApplication.Models.Entities;
using BookstoreApplication.Models.Interfaces;
using BookstoreApplication.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Services.Implementations
{
    public class ComicIssueRepository : Repository<ComicIssue>, IComicIssueRepository
    {
        public ComicIssueRepository(AppDbContext db) : base(db) { }
        public Task<bool> ExistsByExternalIdAsync(long externalId)
            => _db.ComicIssues.AnyAsync(x => x.ExternalIssueId == externalId);
    }
}
