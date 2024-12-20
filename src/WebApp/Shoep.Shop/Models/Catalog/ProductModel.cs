﻿namespace Shoep.Shop.Models.Catalog;

public class ProductModel
{
    private decimal? _discountedPrice;
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryModel? Category { get; set; }
    public List<ProductImage> ProductImages { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public decimal DiscountedPrice
    {
        get => _discountedPrice ?? Price;
        set => _discountedPrice = value;
    }
}

public record ProductImage(Guid Id, Guid ProductId, string ImageUrl, bool IsMain, DateTime CreatedAt);

public record GetProductsResponse(
    IEnumerable<ProductModel> Products,
    long TotalProducts,
    List<CategoryModel> Categories);

public record GetProductByIdResponse(ProductModel Product);