using BookstoreApplication.DTOs.Comics;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IComicsService
    {
        Task<IReadOnlyList<VolumeSearchItemDto>> SearchVolumesAsync(string query);
        Task<IReadOnlyList<IssueSearchItemDto>> GetIssuesByVolumeAsync(long volumeExternalId);
        Task<int> CreateIssueAsync(CreateComicIssueDto input);
    }
}
