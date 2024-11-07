namespace Shoep.Management.Models.Catalog;

public class CreateProductModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string CategoryId { get; set; }
    public List<string> ProductImages { get; set; }
}