namespace BookstoreApplication.Models.Entities
{
    public class ComicIssue
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public DateTime? ReleaseDate { get; set; }
        public string? IssueNumber { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? Description { get; set; }

        public long ExternalIssueId { get; set; }

        public int? PageCount { get; set; }
        public decimal? Price { get; set; }
        public int Stock { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
