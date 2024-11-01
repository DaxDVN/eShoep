namespace Promotion.Application.Dtos;

public class CouponDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public CouponType CouponType { get; set; } = default!;
    public bool IsProductSpecific { get; set; }
    public List<Guid> ProductIds { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
    public bool IsActive { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}