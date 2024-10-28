using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoep.Web.Models.Basket;
using Shoep.Web.Services;

namespace Shoep.Web.Pages
{
    public class CartModel(
        ICatalogService catalogService,
        IBasketService basketService,
        ILogger<ProductListModel> logger) : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAddToCartAsync([FromBody] AddToCartRequest request)
        {
            var productId = new Guid(request.ProductId);
            var qty = request.Qty;
            logger.LogInformation("Add to cart button clicked");

            var productResponse = await catalogService.GetProduct(productId);

            var basket = await basketService.LoadUserBasket();
            var itemExist = basket.Items.FirstOrDefault(item => item.ProductId == productId);
            if (itemExist is null)
            {
                basket.Items.Add(new CartItemModel
                {
                    ProductId = productId,
                    ProductName = productResponse.Product.Name,
                    Price = productResponse.Product.Price,
                    Quantity = qty
                });
            }
            else
            {
                itemExist.Quantity += qty;
            }

            await basketService.StoreBasket(new StoreCartRequest(basket));

            return new JsonResult(new { success = true, message = "Product added to cart!" });
        }

        public record AddToCartRequest(string ProductId, int Qty);
    }
}