using Microsoft.AspNetCore.Mvc;
using Shoep.Management.Interfaces;
using Shoep.Management.Models.Promotion;

namespace Shoep.Management.Controllers
{
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

                if (response != null && response.Coupons.Any())
                {
                    return View(response);
                }
                else
                {
                    ViewBag.Message = "No coupons found";
                    return View(new GetCouponsResponse(new List<Coupon>(), 0));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi: {ex.Message}");
                return View(new GetCouponsResponse(new List<Coupon>(), 0));
            }
        }


        // GET: CouponsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CouponsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CouponsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CouponsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}