using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreApplication.Models.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Biography { get; set; }
        [Column("Birthday")]
        public DateTime DateOfBirth { get; set; }
        public List<AuthorAward> AuthorAwards { get; set; } = new();
    }
}
