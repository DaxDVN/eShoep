using Shoep.Shop.Enums;

namespace Shoep.Shop.Models.Promotion;

public class Coupon
{
    public string Code { get; set; } = default!;
    public PromotionType PromotionType { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Amount { get; set; }
    public DateTime ExpirationDate { get; set; }
}