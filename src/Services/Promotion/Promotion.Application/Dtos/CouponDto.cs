namespace Promotion.Application.Dtos;

public class CouponDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public PromotionType PromotionType { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
    public int MaxRedemptions { get; set; } = 0;
    public int RedemptionCount { get; set; } = 0;
    public DateTime ExpirationDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}