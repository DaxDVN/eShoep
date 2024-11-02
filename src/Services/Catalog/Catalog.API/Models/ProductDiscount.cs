namespace Catalog.API.Models;

public class ProductDiscount
{
    public Guid Id { get; set; }
    public Guid DiscountId { get; set; }
    public Guid ProductId { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
}