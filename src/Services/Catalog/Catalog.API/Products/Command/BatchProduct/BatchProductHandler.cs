using Catalog.API.Repositories;

namespace Catalog.API.Products.Command.BatchProduct;

public record BatchProductCommand : ICommand<BatchProductResult>;

public record BatchProductResult(string Status);

public class BatchProductHandler(IDocumentSession session, ICatalogRepository catalogRepository)
    : ICommandHandler<BatchProductCommand, BatchProductResult>
{
    public async Task<BatchProductResult> Handle(BatchProductCommand request, CancellationToken cancellationToken)
    {
        var batch = await catalogRepository.GetBatchAsync(cancellationToken);
        if (batch is not null) return new BatchProductResult("Already run batch today");

        var discountedProducts = session.Query<Product>()
            .Where(p => p.DiscountedPrice != p.Price)
            .ToDictionary(p => p.Id, p => p);

        if (discountedProducts.Count == 0) return new BatchProductResult("No Event Discount");

        var productIds = discountedProducts.Keys.ToList();

        var expiredDiscounts = await session.Query<ProductDiscount>()
            .Where(pd => productIds.Contains(pd.ProductId)
                         && (pd.IsActive == false || pd.ExpirationDate < DateTime.UtcNow))
            .ToListAsync(cancellationToken);
        if (!expiredDiscounts.Any()) return new BatchProductResult("No product expired");

        foreach (var discount in expiredDiscounts)
        {
            discount.IsActive = false;

            if (discountedProducts.TryGetValue(discount.ProductId, out var product))
                product.DiscountedPrice = product.Price;
        }

        var updatedProducts = expiredDiscounts
            .Select(d => discountedProducts[d.ProductId])
            .ToList();
        var newBatch = new CatalogBatch
        {
            Id = Guid.NewGuid(),
            Status = true,
            RunDate = DateTime.UtcNow
        };

        var result =
            await catalogRepository.StoreCatalogBatch(updatedProducts, expiredDiscounts.ToList(), newBatch,
                cancellationToken);
        return result
            ? new BatchProductResult("Run Batch Successfully")
            : new BatchProductResult("Some thing went wrong");
    }
}