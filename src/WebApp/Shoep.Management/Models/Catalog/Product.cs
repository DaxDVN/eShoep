namespace Shoep.Management.Models.Catalog;

public class Product
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public List<ProductImage> ProductImages { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public record GetProductsResponse(
    IEnumerable<Product> Products,
    long TotalProducts,
    List<Category> Categories);

public record GetProductByIdResponse(Product Product);

public record CreateProductResponse(Guid Id);

public record UpdateProductResponse(bool IsSuccess);

public record DeleteProductResponse(bool IsSuccess);

public record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string CategoryId,
    List<string> ProductImages);

public record UpdateProductRequest(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string CategoryId,
    List<string> ProductImages);