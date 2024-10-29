using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoep.Shop.Models.Basket;
using Shoep.Shop.Services;

namespace Shoep.Shop.Pages;

public class CheckoutModel(
    IBasketService basketService,
    ILogger<ProductListModel> logger) : PageModel
{
    [BindProperty] public CartCheckoutModel Order { get; set; } = default!;
    public CartModel Cart { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var basket = await basketService.LoadUserBasket();
            Cart = basket;
        }
        catch (Exception e)
        {
            Cart = new CartModel
            {
                Items = []
            };
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCheckOutAsync()
    {
        logger.LogInformation("Checkout button clicked");

        Cart = await basketService.LoadUserBasket();

        if (!ModelState.IsValid) return Page();

        // assumption customerId is passed in from the UI authenticated user swn        
        Order.CustomerId = new Guid("58c49479-ec65-4de2-86e7-033c546291aa");
        Order.UserName = Cart.UserName;
        Order.TotalPrice = Cart.TotalPrice;

        await basketService.CheckoutBasket(new CheckoutCartRequest(Order));

        return RedirectToPage("Confirmation", "OrderSubmitted");
    }
}