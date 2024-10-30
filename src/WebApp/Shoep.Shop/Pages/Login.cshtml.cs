using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Shoep.Shop.Models.Auth;
using Shoep.Shop.Services;

namespace Shoep.Shop.Pages;

public class LoginModel(IIdentityService identityService) : PageModel
{
    [BindProperty] public LoginInputModel Input { get; set; }

    public class LoginInputModel
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var loginRequest = new LoginRequest
        {
            Email = Input.Email,
            Password = Input.Password,
            TwoFactorCode = null,
            TwoFactorRecoveryCode = null
        };

        var response = await identityService.LoginAsync(loginRequest);

        if (response.IsSuccessStatusCode)
        {
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
            var accessToken = tokenResponse?.AccessToken;
            var tokenType = tokenResponse?.TokenType;
            if (tokenResponse != null)
            {
                var expiresIn = tokenResponse.ExpiresIn;
            }

            var refreshToken = tokenResponse?.RefreshToken;
            if (accessToken == null) return RedirectToPage("/Index");
            
            var claims = new List<Claim> { new Claim("access_token", accessToken) };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Input.RememberMe
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToPage("/Index");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();
    }
}