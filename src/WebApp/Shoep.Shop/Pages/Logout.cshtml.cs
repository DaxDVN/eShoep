using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shoep.Shop.Pages;

public class LogoutModel : PageModel
{
    public async Task<RedirectToPageResult> OnGetAsync()
    {
        await HttpContext.SignOutAsync("Cookies");

        return RedirectToPage("/Index");
    }
}