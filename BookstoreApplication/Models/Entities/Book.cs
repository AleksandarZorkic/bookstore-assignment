namespace BookstoreApplication.Models.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int PageCount { get; set; }
        public DateTime PublishedDate { get; set; }
        public required string ISBN { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public int PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        public decimal AverageRating { get; set; } = 0m;
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
