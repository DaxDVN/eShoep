namespace Shoep.Management.Models.Purchasing;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;
}