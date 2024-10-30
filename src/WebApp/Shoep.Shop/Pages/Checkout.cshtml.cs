using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoep.Shop.Models.Basket;
using Shoep.Shop.Models.Purchasing;
using Shoep.Shop.Services;

namespace Shoep.Shop.Pages;

public class CheckoutModel(
    IBasketService basketService,
    ILogger<ProductListModel> logger) : PageModel
{
    public CartCheckoutModel Order { get; set; } = default!;
    public CartModel Cart { get; set; } = new();
    [BindProperty] public CustomerCheckout UserInfo { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        if (User.Identity is { IsAuthenticated: false } or null)
        {
            return RedirectToPage("/Login");
        }

        var enumerable = User.Claims as Claim[] ?? User.Claims.ToArray();
        var userId = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        UserInfo.FirstName = enumerable.FirstOrDefault(c => c.Type == "FirstName")?.Value;
        UserInfo.Lastname = enumerable.FirstOrDefault(c => c.Type == "LastName")?.Value;
        UserInfo.AddressLine = enumerable.FirstOrDefault(c => c.Type == "Address")?.Value;
        UserInfo.PhoneNumber = enumerable.FirstOrDefault(c => c.Type == "PhoneNumber")?.Value;
        UserInfo.Email = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        try
        {
            var basket = await basketService.LoadUserBasket(userId!);
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

    public async Task<IActionResult> OnPostCheckoutAsync()
    {
        if (User.Identity is { IsAuthenticated: false } or null)
        {
            return RedirectToPage("/Login");
        }

        logger.LogInformation("Checkout button clicked");

        Cart = await basketService.LoadUserBasket();

        if (!ModelState.IsValid) return RedirectToPage("/Checkout");

        Order.CustomerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
        Order.CustomerName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        Order.TotalPrice = Cart.TotalPrice;
        Order.FirstName = UserInfo.FirstName!;
        Order.LastName = UserInfo.Lastname!;
        Order.EmailAddress = UserInfo.Email!;
        Order.AddressLine = UserInfo.AddressLine!;
        Order.Country = UserInfo.Country;
        Order.State = UserInfo.State;
        Order.ZipCode = UserInfo.ZipCode;
        Order.CardName = UserInfo.CardName;
        Order.CardNumber = UserInfo.CardNumber;
        Order.Expiration = UserInfo.Expiration;
        Order.CVV = UserInfo.Cvv;
        Order.PaymentMethod = 1;

        await basketService.CheckoutBasket(new CheckoutCartRequest(Order));

        return RedirectToPage("Confirmation", "OrderSubmitted");
    }
}