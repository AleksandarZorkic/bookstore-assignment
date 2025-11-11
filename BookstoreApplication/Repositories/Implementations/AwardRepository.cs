using BookstoreApplication.Models.Entities;
using BookstoreApplication.Models.Interfaces;

namespace BookstoreApplication.Repositories.Implementations
{
    public class AwardRepository : Repository<Award>, IAwardRepository
    {
        public AwardRepository(AppDbContext db) : base(db) { }
    }
}
