namespace Catalog.API.Repositories;

public class CatalogRepository(IDocumentSession session) : ICatalogRepository
{
    public async Task<CatalogBatch?> GetBatchAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var latestBatch = await session.Query<CatalogBatch>()
                .OrderByDescending(b => b.RunDate)
                .FirstOrDefaultAsync(cancellationToken);
            return latestBatch?.RunDate == DateTime.UtcNow ? latestBatch : null;
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
            session.Update<Product>(products);

            session.Update<ProductDiscount>(expiredDiscounts);

            session.Store(batch);

            await session.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}