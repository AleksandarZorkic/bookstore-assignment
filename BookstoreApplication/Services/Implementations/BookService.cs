using AutoMapper;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories.Interfaces;
using BookstoreApplication.Services.Interfaces;
using BookstoreApplication.DTOs;
using BookstoreApplication.Exceptions;
using Microsoft.EntityFrameworkCore;
using BookstoreApplication.Services.Implementations;
using Microsoft.Extensions.Logging;

namespace BookstoreApplication.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _books;
        private readonly IAuthorRepository _authors;
        private readonly IPublisherRepository _publishers;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _log;

        public BookService(IBookRepository books, IAuthorRepository authors, IPublisherRepository publishers, IMapper mapper, ILogger<BookService> log)
        {
            _books = books;
            _authors = authors;
            _publishers = publishers;
            _mapper = mapper;
            _log = log;
        }
        public async Task<List<BookDto>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _books.GetAllWithIncludesAsync();
            return _mapper.Map<List<BookDto>>(entities);
        }

        public async Task<BookDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _books.GetOneWithIncludesAsync(id);
            if (entity == null) throw new NotFoundException(id);
            return _mapper.Map<BookDetailsDto>(entity);
        }

        public async Task<Book> CreateAsync(Book dto, CancellationToken ct = default)
        {
            if (!await _authors.ExistsAsync(dto.AuthorId))
                throw new BadRequestException($"Nepostojeci autor {dto.AuthorId}");

            if (!await _publishers.ExistsAsync(dto.PublisherId))
                throw new BadRequestException($"Nepostojeci izdavac {dto.PublisherId}");

            var created = await _books.AddAsync(dto);
            await _books.SaveChangesAsync();

            return (await _books.GetOneWithIncludesAsync(created.Id))!;
        }

        public async Task <Book> UpdateAsync (int id,Book dto, CancellationToken ct = default)
        {
            if (id != dto.Id) throw new BadRequestException("ID u ruti i telu zahteva se razlikuju.");
            if (!await _books.ExistsAsync(id)) throw new NotFoundException(id);
            if (!await _authors.ExistsAsync(dto.AuthorId)) throw new BadRequestException("Autor ne postoji.");
            if (!await _publishers.ExistsAsync(dto.PublisherId)) throw new BadRequestException("Izdavac ne postoji.");

            await _books.UpdateAsync(dto);
            await _books.SaveChangesAsync();
            return (await _books.GetOneWithIncludesAsync(id))!;
        }
        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            if (!await _books.ExistsAsync(id)) throw new NotFoundException(id);

            await _books.DeleteAsync(id);
            try
            {
                await _books.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Operacije nije dozvoljena zbog ogranicenja!");
            }
        }
    }
}
