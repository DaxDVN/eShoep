using Shoep.Management.Enum;

namespace Shoep.Management.Models.Promotion;

public class Discount
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public PromotionType PromotionType { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;
    public List<Guid> ProductIds { get; set; } = [];
    public string? ProductIdsString { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class CreateDiscountRequest
{
    public string Name { get; set; } = string.Empty;
    public string PromotionType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now;
    public List<Guid> ProductIds { get; set; } = [];
    public bool IsActive { get; set; }
}

public record ToggleStatusDiscountRequest(Guid Id, bool IsActive);