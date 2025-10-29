namespace BookstoreApplication.DTOs
{
    public enum BookSortBy { Title, PublishedDate, AuthorName }
    public enum SortDirection { Asc, Desc }

    public class BookSearchRequestDto
    {
        public string? TitleContains { get; set; }
        public DateTime? PublishedFrom { get; set; }
        public DateTime? PublishedTo { get; set; }

        // Pretraga po autoru
        public int? AuthorId { get; set; }
        public string? AuthorNameContains { get; set; }
        public DateTime? AuthorBornFrom { get; set; }
        public DateTime? AuthorBornTo { get; set; }

        // Sort (default: Title Asc)
        public BookSortBy SortBy { get; set; } = BookSortBy.Title;
        public SortDirection Direction { get; set; } = SortDirection.Asc;
    }
}
