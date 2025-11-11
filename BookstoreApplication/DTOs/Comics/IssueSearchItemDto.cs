namespace BookstoreApplication.DTOs.Comics
{
    public record IssueSearchItemDto(
    long ExternalId,
    string Name,
    string? IssueNumber,
    DateTime? CoverDate,
    string? Description,
    string? CoverImageUrl
    );
}
