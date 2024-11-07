using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shoep.Management.Interfaces;
using Shoep.Management.Models.Catalog;

namespace Shoep.Management.Controllers
{
    public class ProductsController(ICatalogService catalogService) : Controller
    {
        // Inject ICatalogService into the controller

        // GET: ProductsController
        public async Task<ActionResult> Index(int pageNumber = 1, int pageSize = 10, int sortType = 1, string name = "",
            string category = "")
        {
            try
            {
                var response = await catalogService.GetProducts(pageNumber, pageSize, sortType, name, category);

                // Pass the total pages and current page to the view
                ViewBag.CurrentPage = pageNumber;
                ViewBag.TotalPages = response.TotalProducts / 10;
                ViewBag.PageSize = pageSize;
                var categoriesJson = JsonConvert.SerializeObject(response.Categories);
                HttpContext.Response.Cookies.Append("Categories", categoriesJson, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7) // Set expiration time for the cookie
                });
                return View(response.Products);
            }
            catch
            {
                return View("Error");
            }
        }


        // GET: ProductsController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            try
            {
                var response = await catalogService.GetProduct(id);
                return View(response.Product);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            var categoriesCookie = HttpContext.Request.Cookies["Categories"];
            if (categoriesCookie == null) return View();
            var categories = JsonConvert.DeserializeObject<List<CategoryModel>>(categoriesCookie);

            if (categories != null) ViewBag.Categories = categories;

            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product.Id = Guid.NewGuid();

                    var temp = new CreateProductModel
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        ProductImages = ["https://paymentcloudinc.com/blog/wp-content/uploads/2021/08/product-ideas-to-sell.jpg"],
                        CategoryId = product.CategoryId.ToString(),
                        StockQuantity = product.StockQuantity,
                    };
                    var response = await catalogService.CreateProduct(temp);
                    if (response.Id != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // GET: ProductsController/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            try
            {
                var response = await catalogService.GetProduct(id);
                if (response.Product != null)
                {
                    var categoriesCookie = HttpContext.Request.Cookies["Categories"];
                    if (categoriesCookie == null) return View();
                    var categories = JsonConvert.DeserializeObject<List<CategoryModel>>(categoriesCookie);

                    if (categories != null) ViewBag.Categories = categories;

                    return View(response.Product);
                }

                return View("Error");
            }
            catch
            {
                return View("Error");
            }
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, ProductModel product)
        {
            try
            {
                var updateProductModel = new UpdateProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    CategoryId = product.CategoryId.ToString(),
                    ProductImages = ["https://paymentcloudinc.com/blog/wp-content/uploads/2021/08/product-ideas-to-sell.jpg"],
                };

                var response = await catalogService.UpdateProduct(updateProductModel);

                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update Fail!");
                }
            }
            catch
            {
                return View("Error");
            }
            return View(product);
        }

        // GET: ProductsController/Delete/5
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var response = await catalogService.GetProduct(id);
                if (response.Product != null)
                {
                    return View(response.Product);
                }

                return View("Error");
            }
            catch
            {
                return View("Error");
            }
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id, ProductModel product)
        {
            try
            {
                var response = await catalogService.DeleteProduct(id);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View("Error");
            }
            catch
            {
                return View("Error");
            }
        }
    }
}