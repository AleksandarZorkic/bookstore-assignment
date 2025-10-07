using BookstoreApplication.Models;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IPublisherService
    {
        Task<List<Publisher>> GetAllAsync(CancellationToken ct = default);
        Task<Publisher?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Publisher> CreateAsync(Publisher dto, CancellationToken ct = default);
        Task UpdateAsync(int id, Publisher dto, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);
    }
}
