using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;

namespace BookstoreApplication.Repositories.Implementations
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(AppDbContext db) : base(db) { }
    }
}
