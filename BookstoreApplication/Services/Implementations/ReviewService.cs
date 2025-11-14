using System;
using System.Threading.Tasks;
using BookstoreApplication.DTOs;
using BookstoreApplication.Models.Entities;
using BookstoreApplication.Models.Interfaces;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviews;
        private readonly IBookRepository _books;
        private readonly IUnitOfWork _uow;

        public ReviewService(IReviewRepository reviews, IBookRepository books, IUnitOfWork uow)
        {
            _reviews = reviews;
            _books = books;
            _uow = uow;
        }

        public async Task<decimal> CreateAsync(string userId, int bookId, CreateReviewDto dto)
        {
            if (dto.Rating < 1 || dto.Rating > 5)
                throw new ArgumentOutOfRangeException(nameof(dto.Rating), "Rating must be 1..5.");

            var book = await _books.GetByIdAsync(bookId)
                       ?? throw new Exception("Book not found.");

            await _uow.BeginTransactionAsync();
            try
            {
                var r = new Review
                {
                    UserId = userId,
                    BookId = bookId,
                    Rating = dto.Rating,
                    Comment = dto.Comment
                };
                await _reviews.AddAsync(r);

                await _uow.SaveAsync();

                var avg = await _reviews.GetAverageForBookAsync(bookId);
                book.AverageRating = Math.Round(avg, 2);

                await _books.UpdateAsync(book);      
                await _uow.CommitAsync();   

                return book.AverageRating;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }
}
