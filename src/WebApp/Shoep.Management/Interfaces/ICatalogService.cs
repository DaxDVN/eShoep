using Refit;
using Shoep.Management.Models.Catalog;

namespace Shoep.Management.Interfaces;

public interface ICatalogService
{
    [Get(
        "/catalog-service/products?pageNumber={pageNumber}&pageSize={pageSize}&sortType={sortType}&name={name}&category={category}")]
    Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10, int? sortType = 1,
        string? name = "", string? category = "");

    [Get("/catalog-service/products/{id}")]
    Task<GetProductByIdResponse> GetProduct(Guid id);

    [Post("/catalog-service/products")]
    Task<CreateProductResponse> CreateProduct(CreateProductModel product);

    [Put("/catalog-service/products")]
    Task<UpdateProductResponse> UpdateProduct(UpdateProductModel product);

    [Delete("/catalog-service/products/{id}")]
    Task<DeleteProductResponse> DeleteProduct(Guid id);
}

public record GetProductsResponse(
    IEnumerable<ProductModel> Products,
    long TotalProducts,
    List<CategoryModel> Categories);

public record GetProductByIdResponse(ProductModel Product);

public record CreateProductResponse(Guid Id);

public record UpdateProductResponse(bool IsSuccess);

public record DeleteProductResponse(bool IsSuccess);