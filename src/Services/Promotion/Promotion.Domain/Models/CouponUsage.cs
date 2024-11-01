namespace Promotion.Domain.Models;

public class CouponUsage
{
    public Guid Id { get; set; }
    public Guid CouponId { get; set; }
    public Guid UserId { get; set; }
    public DateTime UsedAt { get; set; }
}