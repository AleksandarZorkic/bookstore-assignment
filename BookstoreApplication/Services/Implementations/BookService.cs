using AutoMapper;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;
using BookstoreApplication.Services.Interfaces;
using BookstoreApplication.DTOs;

namespace BookstoreApplication.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _books;
        private readonly IAuthorRepository _authors;
        private readonly IPublisherRepository _publishers;
        private readonly IMapper _mapper;

        public BookService(IBookRepository books, IAuthorRepository authors, IPublisherRepository publishers, IMapper mapper)
        {
            _books = books;
            _authors = authors;
            _publishers = publishers;
            _mapper = mapper;
        }
        public async Task<List<BookDto>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _books.GetAllWithIncludesAsync();
            return _mapper.Map<List<BookDto>>(entities);
        }

        public async Task<BookDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _books.GetOneWithIncludesAsync(id);
            return entity is null ? null : _mapper.Map<BookDetailsDto>(entity);
        }

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
