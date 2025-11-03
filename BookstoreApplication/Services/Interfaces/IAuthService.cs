using System.Security.Claims;
using BookstoreApplication.DTOs.UserDto;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegistrationDto data);
        Task<string> LoginAsync(LoginDto data);
        Task LogoutAsync();
        Task <ProfileDto> GetProfileAsync(ClaimsPrincipal userPrincipal);
    }
}
