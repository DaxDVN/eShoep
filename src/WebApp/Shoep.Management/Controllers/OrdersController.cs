using Microsoft.AspNetCore.Mvc;
using Shoep.Management.Interfaces;

namespace Shoep.Management.Controllers;

public class OrdersController(IOrderService orderService) : Controller
{
    // GET: OrdersController
    public async Task<ActionResult> Index(int pageIndex = 1, int pageSize = 5)
    {
        var response = await orderService.GetOrders(new PaginationRequest(pageIndex, pageSize));
        return View(response.Orders);
    }

    // GET: OrdersController/Details/5
    public async Task<ActionResult> Details(string id)
    {
        try
        {
            var response = await orderService.GetOrderById(new Guid(id));
            return View(response.Order);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}