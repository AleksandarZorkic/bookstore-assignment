using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;

namespace BookstoreApplication.Repositories.Implementations
{
    public class PublisherRepository : Repository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(AppDbContext db) : base(db) { }
    }
}
