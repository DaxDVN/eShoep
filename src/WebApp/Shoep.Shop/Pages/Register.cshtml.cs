using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Shoep.Shop.Models.Auth;
using Shoep.Shop.Services;

namespace Shoep.Shop.Pages
{
    public class RegisterModel(IIdentityService identityService) : PageModel
    {
        [BindProperty] public RegisterInputModel Input { get; set; }
        public string RegistrationError { get; set; } = string.Empty;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var registerRequest = new RegisterRequest
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Email = Input.Email,
                PhoneNumber = Input.PhoneNumber,
                Password = Input.Password
            };

            var response = await identityService.RegisterAsync(registerRequest);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Login");
            }

            RegistrationError = "Registration failed. Please check input again.";
            return Page();
        }
    }
}