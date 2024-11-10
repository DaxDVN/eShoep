using Refit;
using Shoep.Shop.Models.Promotion;

namespace Shoep.Shop.Services;

public interface ICouponService
{
    [Get("/promotion-service/promotion/coupons/{userId}/{code}")]
    Task<GetCouponByCodeResponse?> GetCouponByCode(string userId, string code);
}

public record GetCouponByCodeResponse(Coupon Coupon);