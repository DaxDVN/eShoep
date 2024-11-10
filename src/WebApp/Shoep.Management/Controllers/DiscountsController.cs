using Mapster;
using Microsoft.AspNetCore.Mvc;
using Shoep.Management.Interfaces;
using Shoep.Management.Models.Promotion;

namespace Shoep.Management.Controllers;

public class DiscountsController(IDiscountService discountService) : Controller
{
    // GET: CouponsController
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 7)
    {
        try
        {
            var response = await discountService.GetDiscounts(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;

            if (response != null && response.Discounts.Any()) return View(response);

            ViewBag.Message = "No coupons found";
            return View(new GeDiscountsResponse(new List<Discount>(), 0));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi: {ex.Message}");
            return View(new GeDiscountsResponse(new List<Discount>(), 0));
        }
    }

    // GET: DiscountsController/Create
    public IActionResult Create()
    {
        var model = new Discount();
        return View(model);
    }

    // POST: DiscountsController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Discount request)
    {
        ModelState.Remove("ProductIds");
        try
        {
            if (!string.IsNullOrEmpty(request.ProductIdsString))
            {
                var productIds = request.ProductIdsString.Split(',')
                    .Select(id => Guid.Parse(id.Trim()))
                    .ToList();

                request.ProductIds = productIds;
            }
        }
        catch (Exception e)
        {
            return View(request);
        }

        if (!ModelState.IsValid) return View(request);
        try
        {
            var discountRequest = request.Adapt<CreateDiscountRequest>();
            var response = await discountService.CreateDiscount(discountRequest);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi: {ex.Message}");
        }

        return View(request);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(Guid id, bool isActive)
    {
        var request = new ToggleStatusDiscountRequest(id, isActive);
        var response = await discountService.ToggleStatusAsync(request);

        if (response.IsSuccess)
            ViewBag.Message = isActive ? "Discount activated successfully!" : "Discount deactivated successfully!";
        else
            ViewBag.Message = "Failed to update discount status.";

        return RedirectToAction("Index");
    }
}