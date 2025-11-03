using BookstoreApplication.DTOs.UserDto;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegistrationDto data);
        Task LoginAsync(LoginDto data);
        Task LogoutAsync();
    }
}
