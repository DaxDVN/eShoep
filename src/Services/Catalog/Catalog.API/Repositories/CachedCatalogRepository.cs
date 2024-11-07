using Microsoft.Extensions.Caching.Distributed;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Catalog.API.Repositories;

public class CachedCatalogRepository(ICatalogRepository repository, IDistributedCache cache) : ICatalogRepository
{
    public async Task<CatalogBatch?> GetBatchAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var cachedBatch = await cache.GetStringAsync("cachedBatch", cancellationToken);
            if (!string.IsNullOrWhiteSpace(cachedBatch)) return JsonSerializer.Deserialize<CatalogBatch>(cachedBatch);

            var catalogBatch = await repository.GetBatchAsync(cancellationToken);
            if (cachedBatch is not null)
                await cache.SetStringAsync("cachedBatch", JsonSerializer.Serialize(catalogBatch), cancellationToken);

            return catalogBatch;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> StoreCatalogBatch(List<Product> products, List<ProductDiscount> expiredDiscounts,
        CatalogBatch batch,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await repository.StoreCatalogBatch(products, expiredDiscounts, batch, cancellationToken);
            await cache.SetStringAsync("cachedBatch", JsonSerializer.Serialize(batch), cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> ToggleBatch(CancellationToken cancellationToken = default)
    {
        await repository.ToggleBatch(cancellationToken);
        await cache.RemoveAsync("cachedBatch", cancellationToken);
        return true;
    }
}