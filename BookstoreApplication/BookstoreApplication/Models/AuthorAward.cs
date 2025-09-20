using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreApplication.Models
{
    public class AuthorAward
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int AwardId { get; set; }
        public Award Award { get; set; }
        public int YearAwarded { get; set; }
    }
}
