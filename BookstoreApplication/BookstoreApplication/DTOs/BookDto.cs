namespace BookstoreApplication.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string ISBN { get; set; }
        public required string AuthorFullName { get; set; }
        public required string PublisherName { get; set; }
        public int YearsAgo { get; set; }
    }
}
