namespace BookstoreApplication.Models
{
    public class Award
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int StartYear { get; set; }
        public List<AuthorAward> AuthorAwards { get; set; } = new();
    }
}
