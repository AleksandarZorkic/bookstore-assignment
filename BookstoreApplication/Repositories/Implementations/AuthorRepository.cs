using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Repositories.Implementations
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(AppDbContext db) : base(db) { }

        public async Task<(List<Author> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _db.Authors.AsNoTracking().OrderBy(a => a.FullName);
            var total = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            return (items, total);
        }
    }
}
