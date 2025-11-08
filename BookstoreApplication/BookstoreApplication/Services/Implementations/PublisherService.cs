using System.Reflection.Metadata.Ecma335;
using BookstoreApplication.Exceptions;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services.Implementations
{
    public class PublisherService : IPublisherService
    {

        private readonly IPublisherRepository _publishers;

        public PublisherService(IPublisherRepository publishers) => _publishers = publishers;

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
            if (id != dto.Id) throw new BadRequestException("ID mismatch.");
            if (!await _publishers.ExistsAsync(id)) throw new NotFoundException(id);

            await _publishers.UpdateAsync(dto);
            await _publishers.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            if (!await _publishers.ExistsAsync(id)) throw new NotFoundException(id);
            await _publishers.DeleteAsync(id);
            await _publishers.SaveChangesAsync();
        }
        public async Task<List<Publisher>> GetAllSortedAsync(string? sort, CancellationToken ct = default)
        {
            var list = await _publishers.GetAllAsync();

            return (sort ?? "NameAsc").ToLower() switch
            {
                "nameasc" => list.OrderBy(p => p.Name).ToList(),
                "namedesc" => list.OrderByDescending(p => p.Name).ToList(),
                "addressasc" => list.OrderBy(p => p.Address).ToList(),
                "addressdesc" => list.OrderByDescending(p => p.Address).ToList(),
                _ => list.OrderBy(p => p.Name).ToList()
            };
        }
    }
}
