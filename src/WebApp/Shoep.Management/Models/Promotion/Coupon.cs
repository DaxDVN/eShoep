namespace Shoep.Management.Models.Promotion;

public class Coupon
{
    public int Id { get; set; }
    public Guid ProductId { get; set; } = default!;
    public CouponType CouponType { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
}