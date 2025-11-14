using System.Threading.Tasks;
using BookstoreApplication.Models.Entities;

namespace BookstoreApplication.Models.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<decimal> GetAverageForBookAsync(int bookId);
    }
}
