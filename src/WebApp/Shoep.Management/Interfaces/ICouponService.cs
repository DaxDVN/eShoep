using Refit;
using Shoep.Management.Enum;
using Shoep.Management.Models.Promotion;

namespace Shoep.Management.Interfaces;

public interface ICouponService
{
    [Get("/promotion-service/promotion/coupons")]
    Task<GetCouponsResponse> GetCoupons(int? pageNumber = 1, int? pageSize = 7);
    [Post("/promotion-service/promotion/coupons")]
    Task<CreateCouponResponse> CreateCoupon(CreateCouponRequest request);
}
public record GetCouponsResponse(IEnumerable<Coupon> Coupons, long TotalCoupons);
public record CreateCouponResponse(Guid Id);

public class CreateCouponRequest
{
    public string Code { get; set; } = default!;
    public PromotionType PromotionType { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
    public int MaxRedemptions { get; set; } = 0;
    public int RedemptionCount { get; set; } = 0;
    public DateTime ExpirationDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Guid> UserIds { get; set; } = [];
    public bool IsActive { get; set; }
}

