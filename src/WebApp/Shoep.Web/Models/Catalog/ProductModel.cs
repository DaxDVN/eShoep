namespace Shoep.Web.Models.Catalog;

public class ProductModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public CategoryModel CategoryModel { get; set; } = default!;
    public List<ProductImage> ProductImages { get; set; } = default!;
}

public record ProductImage(Guid Id, Guid ProductId, string ImageUrl, bool IsMain, DateTime CreatedAt);

public record GetProductsResponse(IEnumerable<ProductModel> Products);

public record GetProductByCategoryResponse(IEnumerable<ProductModel> Products);

public record GetProductByIdResponse(ProductModel Product);