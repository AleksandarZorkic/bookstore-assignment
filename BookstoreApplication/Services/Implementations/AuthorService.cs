using BookstoreApplication.Exceptions;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authors;

        public AuthorService(IAuthorRepository authors) => _authors = authors;

        public Task<List<Author>> GetAllAsync(CancellationToken ct = default)
            => _authors.GetAllAsync();

        public Task<Author?> GetByIdAsync(int id, CancellationToken ct = default)
            => _authors.GetByIdAsync(id);

        public async Task<Author> CreateAsync(Author dto, CancellationToken ct = default)
        {
            var created = await _authors.AddAsync(dto);
            await _authors.SaveChangesAsync();
            return created;
        }

        public async Task UpdateAsync(int id, Author dto, CancellationToken ct = default)
        {
            if (id != dto.Id) throw new BadRequestException("ID mismatch.");
            if (!await _authors.ExistsAsync(id)) throw new NotFoundException(id);

            await _authors.UpdateAsync(dto);
            await _authors.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            if (!await _authors.ExistsAsync(id)) throw new NotFoundException(id);
            await _authors.DeleteAsync(id);
            await _authors.SaveChangesAsync();
        }
    }
}
