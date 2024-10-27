using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoep.Web.Models.Catalog;
using Shoep.Web.Services;

namespace Shoep.Web.Pages
{
    public class ProductDetailModel(ICatalogService catalogService, ILogger<ProductListModel> logger) : PageModel
    {
        public ProductModel Product { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string code)
        {
            try
            {
                var response = await catalogService.GetProduct(new Guid(code));
                Product = response.Product;
                return Page();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToPage("/ProductList");
            }
        }
        
        // public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        // {
        //     logger.LogInformation("Add to cart button clicked");
        //     var productResponse = await catalogService.GetProduct(productId);
        //
        //     var basket = await basketService.LoadUserBasket();
        //
        //     basket.Items.Add(new ShoppingCartItemModel
        //     {
        //         ProductId = productId,
        //         ProductName = productResponse.Product.Name,
        //         Price = productResponse.Product.Price,
        //         Quantity = 1,
        //         Color = "Black"
        //     });
        //
        //     await basketService.StoreBasket(new StoreBasketRequest(basket));
        //
        //     return RedirectToPage("Cart");
        // }
    }
}