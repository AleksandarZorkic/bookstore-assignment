using Microsoft.AspNetCore.Identity;

namespace BookstoreApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
