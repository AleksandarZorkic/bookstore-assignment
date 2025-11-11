using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookstoreApplication.Services.Interfaces;
using BookstoreApplication.DTOs;
using BookstoreApplication.Exceptions;
using Microsoft.EntityFrameworkCore;
using BookstoreApplication.Services.Implementations;
using Microsoft.Extensions.Logging;
using AutoMapper.QueryableExtensions;
using BookstoreApplication.Models.Interfaces;
using BookstoreApplication.Models.Entities;

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
        public async Task<IEnumerable<BookDto>> GetAllSortedAsync(string? sort)
        {
            var q = _books.QueryWithIncludes(asNoTracking: true);

            var (by, dir) = ParseSort(sort);                
            q = ApplySort(q, (by, dir));

            return await q.ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                          .ToListAsync();
        }

        private static (BookSortBy by, SortDirection dir) ParseSort(string? sort)
        {
            var s = (sort ?? "title_asc").Trim().ToLowerInvariant();
            return s switch
            {
                "title_desc" => (BookSortBy.Title, SortDirection.Desc),
                "date_asc" => (BookSortBy.PublishedDate, SortDirection.Asc),
                "date_desc" => (BookSortBy.PublishedDate, SortDirection.Desc),
                "author_asc" => (BookSortBy.AuthorName, SortDirection.Asc),
                "author_desc" => (BookSortBy.AuthorName, SortDirection.Desc),
                _ => (BookSortBy.Title, SortDirection.Asc),
            };
        }

        private static IQueryable<Book> ApplySort(IQueryable<Book> q, (BookSortBy by, SortDirection dir) s)
        {
            return s.by switch
            {
                BookSortBy.PublishedDate => s.dir == SortDirection.Desc
                    ? q.OrderByDescending(b => b.PublishedDate)
                    : q.OrderBy(b => b.PublishedDate),

                BookSortBy.AuthorName => s.dir == SortDirection.Desc
                    ? q.OrderByDescending(b => b.Author!.FullName)
                    : q.OrderBy(b => b.Author!.FullName),

                _ => s.dir == SortDirection.Desc
                    ? q.OrderByDescending(b => b.Title)
                    : q.OrderBy(b => b.Title),
            };
        }

        public async Task<IEnumerable<BookDto>> SearchAsync(BookSearchRequestDto r)
        {
            var q = _books.QueryWithIncludes(asNoTracking: true);

            if (!string.IsNullOrWhiteSpace(r.TitleContains))
            {
                q = q.Where(b => EF.Functions.ILike(b.Title, $"%{r.TitleContains}%"));
            }
            if (r.PublishedFrom.HasValue)
                q = q.Where(b => b.PublishedDate >= r.PublishedFrom.Value);

            if (r.PublishedTo.HasValue)
                q = q.Where(b => b.PublishedDate <= r.PublishedTo.Value);

            if (r.AuthorId.HasValue)
                q = q.Where(b => b.AuthorId == r.AuthorId.Value);

            if (!string.IsNullOrWhiteSpace(r.AuthorNameContains))
            {
                q = q.Where(b => b.Author != null &&
                                 EF.Functions.ILike(b.Author.FullName, $"%{r.AuthorNameContains}%"));
            }
            if (r.AuthorBornFrom.HasValue)
                q = q.Where(b => b.Author != null && b.Author.DateOfBirth >= r.AuthorBornFrom.Value);

            if (r.AuthorBornTo.HasValue)
                q = q.Where(b => b.Author != null && b.Author.DateOfBirth <= r.AuthorBornTo.Value);

            q = ApplySort(q, (r.SortBy, r.Direction));

            return await q.ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                          .ToListAsync();
        }
    }
}
