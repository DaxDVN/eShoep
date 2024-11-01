namespace Promotion.Domain.Models;

public class Discount
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public PromotionType PromotionType { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Guid> ProductIds { get; set; }
    public bool IsActive { get; set; }
}