// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoep.Management.Models.Auth;
using Shoep.Management.Services;

namespace Shoep.Management.Areas.Identity.Pages.Account;

public class LoginModel(
    SignInManager<User> signInManager,
    ILogger<LoginModel> logger,
    TokenService tokenService,
    UserManager<User> userManager)
    : PageModel
{
    [BindProperty] public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }

    [TempData] public string ErrorMessage { get; set; }

    public async Task OnGetAsync(string returnUrl = null)
    {
        var currentUser = User;

        if (!string.IsNullOrEmpty(ErrorMessage)) ModelState.AddModelError(string.Empty, ErrorMessage);

        returnUrl ??= Url.Content("~/");
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        if (!ModelState.IsValid) return Page();

        var userT = await userManager.FindByEmailAsync(Input.Email);
        if (userT == null || !await userManager.CheckPasswordAsync(userT, Input.Password))
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        logger.LogInformation("User logged in.");

        var user = await signInManager.UserManager.FindByEmailAsync(Input.Email);

        var roles = await userManager.GetRolesAsync(user);
        var roleName = roles.FirstOrDefault();
        if (roleName is not null)
        {
            if (roleName != "Admin")
            {
                return Page();
            }
        }
        var accessToken = await tokenService.GenerateTokenAsync(user);

        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await signInManager.UserManager.UpdateAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? user.Email),
            new("access_token", accessToken)
        };

        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

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

    public class InputModel
    {
        [Required] [EmailAddress] public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}