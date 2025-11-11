namespace BookstoreApplication.DTOs.Comics
{
    public record VolumeSearchItemDto(
    long ExternalId,
    string Name,
    string? Publisher,
    int? StartYear
    );
}
