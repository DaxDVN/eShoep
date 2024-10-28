using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoep.Web.Models.Basket;
using Shoep.Web.Models.Catalog;
using Shoep.Web.Services;

namespace Shoep.Web.Pages;

public class ProductListModel(
    ICatalogService catalogService,
    IBasketService basketService,
    ILogger<ProductListModel> logger)
    : PageModel
{
    public IEnumerable<ProductModel> ProductModels { get; set; } = [];
    public IEnumerable<CategoryModel> CategoryModels { get; set; } = [];
    public long NumberOfPages { get; set; } = 0;
    public int CurrentPage { get; set; } = 1;
    [BindProperty(SupportsGet = true)] public int SortOption { get; set; } = 1;
    [BindProperty(SupportsGet = true)] public string SelectedCategory { get; set; } = "";
    [BindProperty(SupportsGet = true)] public string Name { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int currentPage = 1)
    {
        CurrentPage = currentPage;
        var response = await catalogService.GetProducts(
            pageNumber: CurrentPage,
            pageSize: 6,
            sortType: SortOption,
            name: Name,
            category: SelectedCategory);
        if (response.Products.Any())
        {
            CategoryModels = response.Categories;
        }

        ProductModels = response.Products;
        NumberOfPages = response.TotalProducts / 6 + 1;
        return Page();
    }
}