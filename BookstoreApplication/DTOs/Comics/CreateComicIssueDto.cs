namespace BookstoreApplication.DTOs.Comics
{
    public class CreateComicIssueDto
    {
        public long ExternalIssueId { get; set; }
        public string Title { get; set; } = "";
        public DateTime? ReleaseDate { get; set; }
        public string? IssueNumber { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Description { get; set; }

        // popunjava korisnik
        public int? PageCount { get; set; }
        public decimal? Price { get; set; }
        public int Stock { get; set; } = 0;
    }
}
