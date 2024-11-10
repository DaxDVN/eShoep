using Microsoft.AspNetCore.Mvc;
using Shoep.Management.Interfaces;
using Shoep.Management.Models.Purchasing;

namespace Shoep.Management.Controllers;

public class OrdersController(IOrderService orderService) : Controller
{
    // GET: OrdersController
    public async Task<ActionResult> Index()
    {
        var orders = (await orderService.GetOrders(new PaginationRequest())).Orders.Data.ToList();
        return View(new PaginatedResult<OrderModel>(1, 10, orders.Count, orders));
    }

    // GET: OrdersController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }
}