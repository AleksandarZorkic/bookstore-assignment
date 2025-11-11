using BookstoreApplication.DTOs;
using BookstoreApplication.Models.Entities;

namespace BookstoreApplication.Services
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAsync(CancellationToken ct = default);
        Task<Author?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Author> CreateAsync(Author dto, CancellationToken ct = default);
        Task UpdateAsync(int id, Author dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
        Task<PaginatedResult<Author>> GetPageAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    }
}
