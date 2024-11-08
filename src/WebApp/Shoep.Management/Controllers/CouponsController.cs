using Mapster;
using Microsoft.AspNetCore.Mvc;
using Shoep.Management.Interfaces;
using Shoep.Management.Models.Promotion;

namespace Shoep.Management.Controllers;

public class CouponsController(ICouponService couponService) : Controller
{
    // GET: CouponsController
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 7)
    {
        try
        {
            var response = await couponService.GetCoupons(pageNumber, pageSize);

            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;

            if (response != null && response.Coupons.Any()) return View(response);

            ViewBag.Message = "No coupons found";
            return View(new GetCouponsResponse(new List<Coupon>(), 0));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi: {ex.Message}");
            return View(new GetCouponsResponse(new List<Coupon>(), 0));
        }
    }


    // GET: CouponsController/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var response = await couponService.GetCouponById(id);
            if (response != null)
                return View(response.Coupon);
            return NotFound("Coupon not found");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            return View();
        }
    }

    // GET: CouponsController/Create
    public ActionResult Create()
    {
        var model = new Coupon();
        return View(model);
    }

    // POST: CouponsController/Create
    [HttpPost]
    public async Task<IActionResult> CreateCoupon(CreateCouponRequest request)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var response = await couponService.CreateCoupon(request);
                return Json(new { success = true, message = "Coupon created successfully" });
            }

            return Json(new { success = false });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
        }
    }


    // GET: CouponsController/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var response = await couponService.GetCouponById(id);
            if (response != null)
                return View(response.Coupon.Adapt<UpdateCouponRequest>());
            return NotFound("Coupon not found");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            return View();
        }
    }

    // POST: CouponsController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateCouponRequest request)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var response = await couponService.UpdateCoupon(request);
                if (response.IsSuccess) return RedirectToAction(nameof(Index));

                ModelState.AddModelError(string.Empty, "Failed to update coupon");
                return View(request);
            }

            return View(request);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            return View(request);
        }
    }

    // POST: CouponsController/ToggleStatus
    [HttpPost]
    public async Task<IActionResult> ToggleStatus(Guid id, bool isActive)
    {
        try
        {
            var request = new ToggleStatusCouponRequest(id, isActive);
            var response = await couponService.ToggleStatusCoupon(request);

            if (response.IsSuccess) return RedirectToAction("Index");

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            return RedirectToAction("Index");
        }
    }
}