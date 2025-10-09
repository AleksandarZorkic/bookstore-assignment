namespace BookstoreApplication.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string AuthorFullName { get; set; }
        public string Publisher { get; set; }
        public DateTime PublisheDate { get; set; }
        public int YearsAgo => Math.Max(0, DateTime.Now.Year - PublisheDate.Year);
    }
}
