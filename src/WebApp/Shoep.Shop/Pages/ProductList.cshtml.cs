using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoep.Shop.Models.Catalog;
using Shoep.Shop.Services;

namespace Shoep.Shop.Pages;

public class ProductListModel(
    ICatalogService catalogService,
    IBasketService basketService,
    ILogger<ProductListModel> logger)
    : PageModel
{
    public IEnumerable<ProductModel> ProductModels { get; set; } = [];
    public IEnumerable<CategoryModel> CategoryModels { get; set; } = [];
    public long NumberOfPages { get; set; }
    public int CurrentPage { get; set; } = 1;
    [BindProperty(SupportsGet = true)] public int SortOption { get; set; } = 1;
    [BindProperty(SupportsGet = true)] public string SelectedCategory { get; set; } = "";
    [BindProperty(SupportsGet = true)] public string Name { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int currentPage = 1)
    {
        CurrentPage = currentPage;
        var response = await catalogService.GetProducts(
            CurrentPage,
            6,
            SortOption,
            Name,
            SelectedCategory);
        if (response.Products.Any()) CategoryModels = response.Categories;

        ProductModels = response.Products;
        NumberOfPages = response.TotalProducts / 6 + 1;
        return Page();
    }
}