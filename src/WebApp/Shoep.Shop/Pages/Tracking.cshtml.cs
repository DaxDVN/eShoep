using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shoep.Shop.Models.Basket;
using Shoep.Shop.Models.Purchasing;
using Shoep.Shop.Services;

namespace Shoep.Shop.Pages;

public class TrackingModel(
    IOrderService orderService,
    ILogger<TrackingModel> logger) : PageModel
{
    public List<OrderModel> Orders { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (User.Identity is { IsAuthenticated: false } or null) return RedirectToPage("/Login");

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        try
        {
            var orders = await orderService.GetOrdersByCustomer(userId!);
            Orders = orders.Orders.ToList();
        }
        catch (Exception e)
        {
            Orders = null;
        }

        return Page();
    }
}