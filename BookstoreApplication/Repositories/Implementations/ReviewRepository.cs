using System.Linq;
using System.Threading.Tasks;
using BookstoreApplication.Models.Entities;
using BookstoreApplication.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Repositories.Implementations
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext db) : base(db) { }

        public async Task<decimal> GetAverageForBookAsync(int bookId)
        {
            var query = _db.Set<Review>().Where(r => r.BookId == bookId);
            if (!await query.AnyAsync()) return 0m;
            var avg = await query.AverageAsync(r => r.Rating);
            return (decimal)avg;
        }
    }
}
