using BookstoreApplication.Models.Entities;
using BookstoreApplication.Models.Interfaces;

namespace BookstoreApplication.Repositories.Implementations
{
    public class PublisherRepository : Repository<Publisher>, IPublisherRepository
    {
        public PublisherRepository(AppDbContext db) : base(db) { }
    }
}
