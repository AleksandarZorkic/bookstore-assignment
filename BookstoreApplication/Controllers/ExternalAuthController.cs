using System.Security.Claims;
using BookstoreApplication.Models.Entities;
using BookstoreApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalAuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _auth; 
        private readonly IConfiguration _cfg;

        public ExternalAuthController(
            UserManager<ApplicationUser> userManager,
            IAuthService auth,
            IConfiguration cfg)
        {
            _userManager = userManager;
            _auth = auth;
            _cfg = cfg;
        }

        [HttpGet("google")]
        [AllowAnonymous]
        public IActionResult SignInWithGoogle()
        {
            var redirect = Url.Action(nameof(GoogleCallback), "ExternalAuth", values: null, protocol: Request.Scheme)!;

            var props = new AuthenticationProperties
            {
                RedirectUri = redirect
            };

            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google/callback")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!result.Succeeded || result.Principal == null)
                return Redirect((_cfg["Frontend:BaseUrl"] ?? "http://localhost:5173") + "/login?err=google_failed");

            var extUser = result.Principal;

            var email = extUser.FindFirstValue(ClaimTypes.Email) ?? extUser.FindFirstValue("email");
            var name = extUser.FindFirstValue(ClaimTypes.Name) ?? "Google User";

            if (string.IsNullOrWhiteSpace(email))
                return Redirect((_cfg["Frontend:BaseUrl"] ?? "http://localhost:5173") + "/login?err=email_missing");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    Name = name,
                    EmailConfirmed = true
                };
                var ok = await _userManager.CreateAsync(user);
                if (!ok.Succeeded)
                    return Redirect((_cfg["Frontend:BaseUrl"] ?? "http://localhost:5173") + "/login?err=create_failed");

                // u GoogleCallback, nakon kreiranja/pronalazenja user-a:
                var info = new UserLoginInfo("Google",
                    extUser.FindFirstValue(ClaimTypes.NameIdentifier) ?? "", "Google");
                var hasLogin = (await _userManager.GetLoginsAsync(user))
                                  .Any(l => l.LoginProvider == "Google");
                if (!hasLogin) await _userManager.AddLoginAsync(user, info);

            }

            var jwt = await _auth.IssueJwtAsync(user);

            // očisti eksterni cookie
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var fe = (_cfg["Frontend:BaseUrl"] ?? "http://localhost:5173").TrimEnd('/');
            return Redirect($"{fe}/oauth-callback#token={Uri.EscapeDataString(jwt)}");
        }
    }
}
