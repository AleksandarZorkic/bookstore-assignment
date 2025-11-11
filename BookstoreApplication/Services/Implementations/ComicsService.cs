using BookstoreApplication.DTOs.Comics;
using BookstoreApplication.Models.Interfaces;
using BookstoreApplication.Models.Entities;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services.Implementations
{
    public class ComicsService : IComicsService
    {
        private readonly IComicsClient _client;
        private readonly IComicIssueRepository _repo;

        public ComicsService(IComicsClient client, IComicIssueRepository repo)
        {
            _client = client;
            _repo = repo;
        }

        public Task<IReadOnlyList<VolumeSearchItemDto>> SearchVolumesAsync(string query)
            => _client.SearchVolumesAsync(query);

        public Task<IReadOnlyList<IssueSearchItemDto>> GetIssuesByVolumeAsync(long volumeExternalId)
            => _client.GetIssuesByVolumeAsync(volumeExternalId);

        public async Task<int> CreateIssueAsync(CreateComicIssueDto input)
        {
            if (await _repo.ExistsByExternalIdAsync(input.ExternalIssueId))
                throw new Exception("Issue already saved");

            var e = new ComicIssue
            {
                ExternalIssueId = input.ExternalIssueId,
                Title = input.Title,
                ReleaseDate = input.ReleaseDate,
                IssueNumber = input.IssueNumber,
                CoverImageUrl = input.CoverImageUrl,
                Description = input.Description,
                PageCount = input.PageCount,
                Price = input.Price,
                Stock = input.Stock
            };
            await _repo.AddAsync(e);
            await _repo.SaveChangesAsync();
            return e.Id;
        }
    }
}
