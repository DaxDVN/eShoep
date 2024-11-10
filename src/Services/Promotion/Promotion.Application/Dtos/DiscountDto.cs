namespace Promotion.Application.Dtos;

public class DiscountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string PromotionType { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Guid> ProductIds { get; set; } = new();
    public bool IsActive { get; set; }
}