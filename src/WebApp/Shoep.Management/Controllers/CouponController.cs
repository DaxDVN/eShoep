using Microsoft.AspNetCore.Mvc;

namespace Shoep.Management.Controllers;

public class CouponController : Controller
{
    // GET: CouponController
    public ActionResult Index()
    {
        return View();
    }

    // GET: CouponController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: CouponController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: CouponController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
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

    // GET: CouponController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: CouponController/Edit/5
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

    // GET: CouponController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: CouponController/Delete/5
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