using BookstoreApplication.Models;

namespace BookstoreApplication.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public  AuthorRepository(AppDbContext db) : base(db) { }
    }
}
