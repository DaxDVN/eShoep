using Microsoft.AspNetCore.Mvc;
using Shoep.Management.Interfaces;
using Shoep.Management.Models.Catalog;

namespace Shoep.Management.Controllers
{
    public class ProductsController(ICatalogService catalogService) : Controller
    {
        // Inject ICatalogService into the controller

        // GET: ProductsController
        public async Task<ActionResult> Index(int pageNumber = 1, int pageSize = 10, int sortType = 1, string name = "", string category = "")
        {
            try
            {
                var response = await catalogService.GetProducts(pageNumber, pageSize, sortType, name, category);

                // Pass the total pages and current page to the view
                ViewBag.CurrentPage = pageNumber;
                ViewBag.TotalPages = response.TotalProducts / 10;
                ViewBag.PageSize = pageSize;

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
                    var response = await catalogService.CreateProduct(product);
                    if (response.Id != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View();
            }
            catch
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
                if (ModelState.IsValid)
                {
                    var response = await catalogService.UpdateProduct(true); // Assuming true is passed for success
                    if (response.Success == true)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(product);
            }
            catch
            {
                return View("Error");
            }
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
                var response = await catalogService.DeleteProduct(true); // Assuming true for successful deletion
                if (response.Success != null)
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