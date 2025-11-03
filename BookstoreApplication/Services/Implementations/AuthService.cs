using BookstoreApplication.DTOs.UserDto;
using BookstoreApplication.Models;
using Microsoft.AspNetCore.Identity;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> um, SignInManager<ApplicationUser> sm)
        {
            _userManager = um;
            _signInManager = sm;
        }

        public async Task RegisterAsync(RegistrationDto data)
        {
            var user = new ApplicationUser
            {
                UserName = data.UserName,
                Email = data.Email,
                Name = data.Name,
                Surname = data.Surname,
            };

            var result = await _userManager.CreateAsync(user, data.Password);
            if (!result.Succeeded)
            {
                var msg = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new BadHttpRequestException(msg);
            }

            await _signInManager.SignInAsync(user, isPersistent: true);
        }

        public async Task LoginAsync(LoginDto data)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userName: data.Username,
                password: data.Password,
                isPersistent: true,
                lockoutOnFailure: false);

            if (!result.Succeeded)
                throw new BadHttpRequestException("Invalid credentials.");
        }

        public async Task LogoutAsync() => _signInManager.SignOutAsync();
    }
        
}
