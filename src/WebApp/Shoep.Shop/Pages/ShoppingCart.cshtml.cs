using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoep.Shop.Models.Basket;
using Shoep.Shop.Pages;
using Shoep.Shop.Services;

namespace Shoep.Shop.Pages;

public class ShoppingCartModel(
    ICatalogService catalogService,
    IBasketService basketService,
    ILogger<ProductListModel> logger) : PageModel
{
    public CartModel Cart { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        if (User.Identity is { IsAuthenticated: false } or null)
        {
            return RedirectToPage("/Login");
        }

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
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

    public async Task<IActionResult> OnPostAddToCartAsync([FromBody] AddToCartRequest request)
    {
        if (User.Identity is { IsAuthenticated: false } or null)
        {
            return new JsonResult(new { success = false, message = "Please login" });
        }

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        var productId = new Guid(request.ProductId);
        var qty = request.Qty;
        logger.LogInformation("Add to cart button clicked");

        var productResponse = await catalogService.GetProduct(productId);

        var basket = await basketService.LoadUserBasket(userId!);
        if (string.IsNullOrEmpty(basket.UserId))
        {
            basket.UserId = userId!;
        }

        var itemExist = basket.Items.FirstOrDefault(item => item.ProductId == productId);
        if (itemExist is null)
            basket.Items.Add(new CartItemModel
            {
                ProductId = productId,
                ProductName = productResponse.Product.Name,
                Price = productResponse.Product.Price,
                Quantity = qty
            });
        else
            itemExist.Quantity += qty;

        await basketService.StoreBasket(new StoreCartRequest(basket));

        return new JsonResult(new { success = true, message = "Product added to cart!" });
    }

    public record AddToCartRequest(string ProductId, int Qty);
}