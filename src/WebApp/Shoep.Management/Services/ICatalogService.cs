using Refit;
using Shoep.Management.Models.Catalog;

namespace Shoep.Management.Services;

public interface ICatalogService
{
    [Get(
        "/catalog-service/products?pageNumber={pageNumber}&pageSize={pageSize}&sortType={sortType}&name={name}&category={category}")]
    Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10, int? sortType = 1,
        string? name = "", string? category = "");

    [Get("/catalog-service/products/{id}")]
    Task<GetProductByIdResponse> GetProduct(Guid id);

    [Post("/catalog-service/products")]
    Task<CreateProductResponse> CreateProduct([Body] CreateProductRequest request);

    [Put("/catalog-service/products")]
    Task<UpdateProductResponse> UpdateProduct([Body] UpdateProductRequest request);

    [Delete("/catalog-service/products/{id}")]
    Task<DeleteProductResponse> DeleteProduct(Guid id);
}