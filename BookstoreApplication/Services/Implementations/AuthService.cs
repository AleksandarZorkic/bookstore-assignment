using BookstoreApplication.DTOs.UserDto;
using BookstoreApplication.Models;
using Microsoft.AspNetCore.Identity;
using BookstoreApplication.Services.Interfaces;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookstoreApplication.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(UserManager<ApplicationUser> um, SignInManager<ApplicationUser> sm, IConfiguration configuration, IMapper mapper)
        {
            _userManager = um;
            _signInManager = sm;
            _configuration = configuration;
            _mapper = mapper;
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
        }

        public async Task<string> LoginAsync(LoginDto data)
        {
            var user = await _userManager.FindByNameAsync(data.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, data.Password))
                throw new BadHttpRequestException("Invalid credentials.");

            return await GenerateJwt(user);
        }
        public async Task LogoutAsync() => _signInManager.SignOutAsync();

        private async Task<string> GenerateJwt(ApplicationUser user)
            {
                var claims = new List<Claim>
                {
                  new Claim(JwtRegisteredClaimNames.Sub, user.Id),  
                  new Claim("username", user.UserName),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
                };


                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                  issuer: _configuration["Jwt:Issuer"],
                  audience: _configuration["Jwt:Audience"],
                  claims: claims,
                  expires: DateTime.UtcNow.AddDays(1), 
                  signingCredentials: creds
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ProfileDto> GetProfileAsync(ClaimsPrincipal userPrincipal)
        {
            var username = userPrincipal.FindFirstValue("username");

            if (username == null)
                throw new BadHttpRequestException("Invalid user.");

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                throw new BadHttpRequestException("User not found.");

            return _mapper.Map<ProfileDto>(user);
        }
    }      
}
