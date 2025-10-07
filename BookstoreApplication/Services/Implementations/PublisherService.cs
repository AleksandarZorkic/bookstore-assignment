using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services.Implementations
{
    public class PublisherService : IPublisherService
    {

        private readonly IPublisherRepository _publishers;

        public PublisherService(IPublisherRepository authors) => _publishers = authors;

        public Task<List<Publisher>> GetAllAsync(CancellationToken ct = default)
            => _publishers.GetAllAsync();

        public Task<Publisher?> GetByIdAsync(int id, CancellationToken ct = default)
            => _publishers.GetByIdAsync(id);

        public async Task<Publisher> CreateAsync(Publisher dto, CancellationToken ct = default)
        {
            var created = await _publishers.AddAsync(dto);
            await _publishers.SaveChangesAsync();
            return created;
        }

        public async Task UpdateAsync(int id, Publisher dto, CancellationToken ct = default)
        {
            if (id != dto.Id) throw new ArgumentException("ID mismatch.");
            if (!await _publishers.ExistsAsync(id)) throw new KeyNotFoundException();

            await _publishers.UpdateAsync(dto);
            await _publishers.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _publishers.DeleteAsync(id);
            await _publishers.SaveChangesAsync();
        }
    }
}
