using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Shoep.Shop.Enums;
using Shoep.Shop.Models.Basket;
using Shoep.Shop.Models.Promotion;
using Shoep.Shop.Models.Purchasing;
using Shoep.Shop.Services;

namespace Shoep.Shop.Pages;

public class CheckoutModel(
    IBasketService basketService,
    ICouponService couponService,
    ILogger<ProductListModel> logger) : PageModel
{
    public CartCheckoutModel Order { get; set; } = new();
    public CartModel Cart { get; set; } = new();
    public string CouponCode { get; set; }
    [BindProperty] public CustomerCheckout UserInfo { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        if (User.Identity is { IsAuthenticated: false } or null) return RedirectToPage("/Login");

        var enumerable = User.Claims as Claim[] ?? User.Claims.ToArray();
        var userId = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        UserInfo.FirstName = enumerable.FirstOrDefault(c => c.Type == "FirstName")?.Value;
        UserInfo.Lastname = enumerable.FirstOrDefault(c => c.Type == "LastName")?.Value;
        UserInfo.AddressLine = enumerable.FirstOrDefault(c => c.Type == "Address")?.Value;
        UserInfo.PhoneNumber = enumerable.FirstOrDefault(c => c.Type == "PhoneNumber")?.Value;
        UserInfo.Email = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        try
        {
            var basket = await basketService.LoadUserBasket(userId!);
            Cart = basket;
        }
        catch (Exception e)
        {
            Cart = new CartModel
            {
                Items = []
            };
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCheckoutAsync()
    {
        if (User.Identity is { IsAuthenticated: false } or null) return RedirectToPage("/Login");

        logger.LogInformation("Checkout button clicked");

        var enumerable = User.Claims as Claim[] ?? User.Claims.ToArray();
        var userId = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        Cart = await basketService.LoadUserBasket(userId!);
        if (!ModelState.IsValid) return Page();

        Order.CustomerId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
        Order.CustomerName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        Order.TotalPrice = Cart.TotalPrice;

        var coupon = JsonConvert.DeserializeObject<Coupon>(TempData["Coupon"]?.ToString() ?? string.Empty);
        var orderCouponApply = coupon?.Code;
        if (orderCouponApply != null)
            Order.CouponApply = orderCouponApply;

        Order.FirstName = UserInfo.FirstName!;
        Order.LastName = UserInfo.Lastname!;
        Order.EmailAddress = UserInfo.Email!;
        Order.AddressLine = UserInfo.AddressLine!;
        Order.Country = UserInfo.Country;
        Order.State = UserInfo.State;
        Order.ZipCode = UserInfo.ZipCode;

        Order.CardName = UserInfo.CardName;
        Order.CardNumber = UserInfo.CardNumber;
        Order.Expiration = UserInfo.Expiration;
        Order.CVV = UserInfo.Cvv;
        Order.PaymentMethod = 1;

        await basketService.CheckoutBasket(new CheckoutCartRequest(Order));

        if (coupon != null)
        {
            var maxDiscountAmount = Order.TotalPrice * 3 / 10;


            if (coupon.PromotionType == PromotionType.Percentage)
            {
                var discountAmount = Order.TotalPrice * coupon.Amount / 100;

                if (discountAmount > maxDiscountAmount) discountAmount = maxDiscountAmount;

                Order.TotalPrice -= discountAmount;
            }
            else
            {
                var discountAmount = (decimal)coupon.Amount;

                if (discountAmount > maxDiscountAmount) discountAmount = maxDiscountAmount;

                Order.TotalPrice -= discountAmount;
            }
        }

        TempData["Cart"] = JsonConvert.SerializeObject(Cart);
        TempData["Order"] = JsonConvert.SerializeObject(Order);

        return RedirectToPage("/Confirmation");
    }

    public async Task<JsonResult> OnPostApplyCouponAsync([FromBody] string couponCode)
    {
        if (string.IsNullOrEmpty(couponCode))
            return new JsonResult(new { isValid = false, message = "Coupon code is required." });

        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                Cart = await basketService.LoadUserBasket(userId);
                var coupon = await couponService.GetCouponByCode(userId, couponCode);
                if (coupon != null)
                {
                    TempData["Coupon"] = JsonConvert.SerializeObject(coupon.Coupon);
                    await basketService.StoreBasket(new StoreCartRequest(Cart));
                    return new JsonResult(new
                    {
                        isValid = true, coupon.Coupon,
                        promotionType = coupon.Coupon.PromotionType.ToString()
                    });
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return new JsonResult(new { isValid = false, message = "Coupon is invalid." });
    }


    public async Task<JsonResult> OnPostCancelCouponAsync()
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                Cart = await basketService.LoadUserBasket(userId);
                return new JsonResult(new { isValid = true });
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return new JsonResult(new { isValid = false });
    }
}