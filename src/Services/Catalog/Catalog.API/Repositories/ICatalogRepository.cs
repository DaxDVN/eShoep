namespace Catalog.API.Repositories;

public interface ICatalogRepository
{
    Task<CatalogBatch?> GetBatchAsync(CancellationToken cancellationToken = default);

    Task<bool> StoreCatalogBatch(List<Product> products, List<ProductDiscount> expiredDiscounts,
        CatalogBatch batch, CancellationToken cancellationToken = default);
}