namespace Discount.API.Models;

public class Coupon
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; } = default!;
    public CouponType CouponType { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
}