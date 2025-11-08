using BookstoreApplication.DTOs;
using BookstoreApplication.Models;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IBookService
    {
        Task<List<BookDto>> GetAllAsync(CancellationToken ct = default);
        Task<BookDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Book> CreateAsync(Book dto, CancellationToken ct = default);
        Task<Book> UpdateAsync(int id, Book dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<IEnumerable<BookDto>> GetAllSortedAsync(string? sort);  
        Task<IEnumerable<BookDto>> SearchAsync(BookSearchRequestDto req);
    }
}
