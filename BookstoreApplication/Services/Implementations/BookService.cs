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
            _log.LogInformation("Fetching all books");
            var entities = await _books.GetAllWithIncludesAsync();
            _log.LogInformation("Fetched {Count} books", entities.Count);
            return _mapper.Map<List<BookDto>>(entities);
        }

        public async Task<BookDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            _log.LogInformation("Fetching book by id {BookId}", id);
            var entity = await _books.GetOneWithIncludesAsync(id);
            if (entity is null)
            {
                _log.LogWarning("Book not found: id={BookId}", id);
                throw new NotFoundException(id);
            }
            return _mapper.Map<BookDetailsDto>(entity);
        }

        public async Task<Book> CreateAsync(Book dto, CancellationToken ct = default)
        {
            _log.LogInformation("Creating book {Title} (AuthorId={AuthorId}, PublisherId={PublisherId})",
            dto.Title, dto.AuthorId, dto.PublisherId);

            if (!await _authors.ExistsAsync(dto.AuthorId))
            {
                _log.LogWarning("Create failed: author does not exist (AuthorId={AuthorId})", dto.AuthorId);
                throw new BadRequestException($"Autor (id={dto.AuthorId}) ne postoji.");
            }
            if (!await _publishers.ExistsAsync(dto.PublisherId))
            {
                _log.LogWarning("Create failed: publisher does not exist (PublisherId={PublisherId})", dto.PublisherId);
                throw new BadRequestException($"Izdavač (id={dto.PublisherId}) ne postoji.");
            }

            var created = await _books.AddAsync(dto);
            await _books.SaveChangesAsync();

            _log.LogInformation("Book created with id {BookId}", created.Id);
            return (await _books.GetOneWithIncludesAsync(created.Id))!;
        }

        public async Task <Book> UpdateAsync (int id,Book dto, CancellationToken ct = default)
        {
            _log.LogInformation("Updating book id={BookId}", id);

            if (id != dto.Id)
            {
                _log.LogWarning("Update failed: route id {RouteId} != body id {BodyId}", id, dto.Id);
                throw new BadRequestException("ID u ruti i telu zahteva se razlikuju.");
            }
            if (!await _books.ExistsAsync(id))
            {
                _log.LogWarning("Update failed: book not found (id={BookId})", id);
                throw new NotFoundException(id);
            }
            if (!await _authors.ExistsAsync(dto.AuthorId))
            {
                _log.LogWarning("Update failed: author does not exist (AuthorId={AuthorId})", dto.AuthorId);
                throw new BadRequestException("Autor ne postoji.");
            }
            if (!await _publishers.ExistsAsync(dto.PublisherId))
            {
                _log.LogWarning("Update failed: publisher does not exist (PublisherId={PublisherId})", dto.PublisherId);
                throw new BadRequestException("Izdavac ne postoji.");
            }

            await _books.UpdateAsync(dto);
            await _books.SaveChangesAsync();

            _log.LogInformation("Book updated id={BookId}", id);
            return (await _books.GetOneWithIncludesAsync(id))!;
        }
        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            _log.LogInformation("Deleting book id={BookId}", id);

            if (!await _books.ExistsAsync(id))
            {
                _log.LogWarning("Delete failed: book not found (id={BookId})", id);
                throw new NotFoundException(id);
            }

            await _books.DeleteAsync(id);
            try
            {
                await _books.SaveChangesAsync();
                _log.LogInformation("Book deleted id={BookId}", id);
            }
            catch (DbUpdateException ex)
            {
                _log.LogError(ex, "Delete failed due to DB constraint (id={BookId})", id);
                throw new ConflictException("Operacija nije dozvoljena zbog referencijalnih ograničenja.");
            }
        }
    }
}
