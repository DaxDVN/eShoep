using Mapster;
using Microsoft.AspNetCore.Mvc;
using Shoep.Management.Models.Catalog;
using Shoep.Management.Services;

namespace Shoep.Management.Controllers;

public class ProductController(ICatalogService catalogService, ILogger<ProductController> logger)
    : Controller
{
    // GET: Product/Index
    public async Task<IActionResult> Index(int? pageNumber = 1, int? pageSize = 10, int? sortType = 1,
        string? name = "", string? category = "")
    {
        try
        {
            var response = await catalogService.GetProducts(pageNumber, pageSize, sortType, name, category);
            ViewBag.Categories = response.Categories;
            return View(response.Products);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading products");
            return View("Error");
        }
    }

    // GET: Product/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var response = await catalogService.GetProduct(id);
            return View(response.Product);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading product details");
            return View("Error");
        }
    }

    // GET: Product/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        if (!ModelState.IsValid) return View(request.Adapt<Product>());

        try
        {
            var response = await catalogService.CreateProduct(request);
            return RedirectToAction(nameof(Details), new { id = response.Id });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating product");
            return View("Error");
        }
    }

    // GET: Product/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var response = await catalogService.GetProduct(id);
            var product = response.Product;

            var updateRequest = new UpdateProductRequest(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.StockQuantity,
                product.CategoryId.ToString(),
                product.ProductImages.Select(img => img.ImageUrl).ToList()
            );

            return View(updateRequest);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading product for editing");
            return View("Error");
        }
    }

    // POST: Product/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateProductRequest request)
    {
        if (!ModelState.IsValid) return View(request);

        try
        {
            var response = await catalogService.UpdateProduct(request);
            return RedirectToAction(nameof(Details), new { id = request.Id });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating product");
            return View("Error");
        }
    }

    // GET: Product/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var response = await catalogService.GetProduct(id);
            return View(response.Product);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading product for deletion");
            return View("Error");
        }
    }

    // POST: Product/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            var response = await catalogService.DeleteProduct(id);
            if (response.IsSuccess) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", "Failed to delete product");
            return RedirectToAction(nameof(Delete));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting product");
            return View("Error");
        }
    }
}