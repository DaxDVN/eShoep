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
    Task<GetProductCreate> CreateProduct(ProductModel Product);

    [Put("/catalog-service/products")]
    Task<GetProductUpdate> UpdateProduct(bool succcess);

    [Delete("/catalog-service/products")]
    Task<GetProductDelete> DeleteProduct(bool succcess);
}