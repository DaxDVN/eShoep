using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Shoep.Shop.Models.Basket;

namespace Shoep.Shop.Pages;

public class ConfirmationModel : PageModel
{
    public CartModel? Cart { get; set; }
    public CartCheckoutModel? Order { get; set; }

    public IActionResult OnGet()
    {
        if (TempData["Cart"] == null || TempData["Order"] == null) return RedirectToPage("/Index");
        Cart = JsonConvert.DeserializeObject<CartModel>(TempData["Cart"]?.ToString() ?? string.Empty);
        Order = JsonConvert.DeserializeObject<CartCheckoutModel>(TempData["Order"]?.ToString() ?? string.Empty);
        return Page();
    }
}