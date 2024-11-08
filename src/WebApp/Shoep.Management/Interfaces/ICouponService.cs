using Refit;
using Shoep.Management.Models.Promotion;

namespace Shoep.Management.Interfaces;

public interface ICouponService
{
    [Get("/promotion-service/promotion/coupons")]
    Task<GetCouponsResponse> GetCoupons(int? pageNumber = 1, int? pageSize = 7);

    [Post("/promotion-service/promotion/coupons")]
    Task<CreateCouponResponse> CreateCoupon(CreateCouponRequest request);

    [Get("/promotion-service/promotion/coupons/{id}")]
    Task<GetCouponByIdResponse> GetCouponById(Guid id);

    [Put("/promotion-service/promotion/coupons")]
    Task<UpdateCouponResponse> UpdateCoupon(UpdateCouponRequest request);

    [Put("/promotion-service/promotion/coupons/toggle-status")]
    Task<ToggleStatusCouponResponse> ToggleStatusCoupon(ToggleStatusCouponRequest request);
}

public record GetCouponsResponse(IEnumerable<Coupon> Coupons, long TotalCoupons);

public record CreateCouponResponse(Guid Id);

public record GetCouponByIdResponse(Coupon Coupon);

public record UpdateCouponResponse(bool IsSuccess);

public record ToggleStatusCouponResponse(bool IsSuccess);