using BookstoreApplication.DTOs.Comics;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IComicsClient
    {
        Task<IReadOnlyList<VolumeSearchItemDto>> SearchVolumesAsync(string query, int limit = 20);
        Task<IReadOnlyList<IssueSearchItemDto>> GetIssuesByVolumeAsync(long volumeExternalId, int limit = 50);
    }
}
