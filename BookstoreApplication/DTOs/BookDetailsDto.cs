namespace BookstoreApplication.DTOs
{
    public class BookDetailsDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int PageCount { get; set; }
        public DateTime PublishedDate { get; set; }
        public required string ISBN { get; set; }
        public int AuthorId { get; set; }
        public required string AuthorFullName { get; set; }
        public int PublisherId { get; set; }
        public required string PublisherName { get; set; }
    }
}
