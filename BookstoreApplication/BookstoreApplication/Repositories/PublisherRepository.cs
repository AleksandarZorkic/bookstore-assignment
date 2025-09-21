using BookstoreApplication.Models;

namespace BookstoreApplication.Repositories
{
    public class PublisherRepository : Repository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(AppDbContext db) : base(db) { }
    }
}
