using System.ComponentModel.DataAnnotations;

namespace BookstoreApplication.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public int BookId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigacije
        public ApplicationUser? User { get; set; }
        public Book? Book { get; set; }

    }
}
