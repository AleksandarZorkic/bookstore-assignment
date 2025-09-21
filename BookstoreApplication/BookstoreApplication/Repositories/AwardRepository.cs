using BookstoreApplication.Models;

namespace BookstoreApplication.Repositories
{
    public class AwardRepository : Repository<Award>, IAwardRepository
    {
        public AwardRepository(AppDbContext db) : base(db) { }
    }
}
