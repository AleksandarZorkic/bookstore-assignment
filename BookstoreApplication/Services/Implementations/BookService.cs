using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _books;
        private readonly IAuthorRepository _authors;
        private readonly IPublisherRepository _publishers;

        public BookService(IBookRepository books, IAuthorRepository authors, IPublisherRepository publishers)
        {
            _books = books;
            _authors = authors;
            _publishers = publishers;
        }
        public Task<List<Book>> GetAllAsync(CancellationToken ct = default)
            => _books.GetAllWithIncludesAsync();

        public Task<Book?> GetByIdAsync(int id, CancellationToken ct = default)
            => _books.GetOneWithIncludesAsync(id);

        public async Task<Book> CreateAsync(Book dto, CancellationToken ct = default)
        {
            if (!await _authors.ExistsAsync(dto.AuthorId))
                throw new ArgumentException($"Nepostojeci autor {dto.AuthorId}");

            if (!await _publishers.ExistsAsync(dto.PublisherId))
                throw new ArgumentException($"Nepostojeci izdavac {dto.PublisherId}");

            var created = await _books.AddAsync(dto);
            await _books.SaveChangesAsync();

            return (await _books.GetOneWithIncludesAsync(created.Id))!;
        }

        public async Task <Book> UpdateAsync (int id,Book dto, CancellationToken ct = default)
        {
            if (id != dto.Id) throw new ArgumentException("ID mismatch.");
            if (!await _books.ExistsAsync(id)) throw new KeyNotFoundException();
            if (!await _authors.ExistsAsync(dto.AuthorId)) throw new KeyNotFoundException();
            if (!await _publishers.ExistsAsync(dto.PublisherId)) throw new KeyNotFoundException();

            await _books.UpdateAsync(dto);
            await _books.SaveChangesAsync();

            return (await _books.GetOneWithIncludesAsync(id))!;
        }
        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _books.DeleteAsync(id);
            await _books.SaveChangesAsync();
        }
    }
}
