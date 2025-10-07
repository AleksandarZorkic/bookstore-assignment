using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;

namespace BookstoreApplication.Repositories.Implementations
{
    public class AwardRepository : Repository<Award>, IAwardRepository
    {
        public AwardRepository(AppDbContext db) : base(db) { }
    }
}
