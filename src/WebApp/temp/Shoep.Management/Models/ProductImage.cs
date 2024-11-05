namespace Shoep.Management.Models;

public class ProductImage
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsMain { get; set; }
    public DateTime CreatedAt { get; set; }
}