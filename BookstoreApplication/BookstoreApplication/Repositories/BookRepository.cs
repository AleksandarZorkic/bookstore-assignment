using BookstoreApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext db) : base(db) { }

        public Task<List<Book>> GetAllWithIncludesAsync(bool asNoTracking = true)
        {
            var q = _db.Books.Include(b => b.Author)
                            .Include(b => b.Publisher);
            return (asNoTracking ? q.AsNoTracking() : q).ToListAsync();
        }

        public Task<Book?> GetOneWithIncludesAsync(int id, bool asNoTracking = true)
        {
            var q = _db.Books.Include(b => b.Author)
                            .Include(b => b.Publisher)
                            .Where(b => b.Id == id);
            return (asNoTracking ? q.AsNoTracking() : q).FirstOrDefaultAsync();
        }
    }
}
