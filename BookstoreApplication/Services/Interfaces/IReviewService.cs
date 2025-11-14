using System.Threading.Tasks;
using BookstoreApplication.DTOs;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IReviewService
    {
        Task<decimal> CreateAsync(string userId, int bookId, CreateReviewDto dto);
    }
}
