using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoep.Web.Models.Basket;
using Shoep.Web.Models.Catalog;
using Shoep.Web.Services;

namespace Shoep.Web.Pages
{
    public class ProductDetailModel(
        ICatalogService catalogService) : PageModel
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
    }
}