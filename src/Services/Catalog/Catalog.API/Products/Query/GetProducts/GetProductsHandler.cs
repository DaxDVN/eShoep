using Common.Messaging.Events;
using Marten.Linq;
using MassTransit;

namespace Catalog.API.Products.Query.GetProducts;

public record GetProductsQuery(
    int? PageNumber = 1,
    int? PageSize = 3,
    int? SortType = 1,
    string? Name = "",
    string? Category = "")
    : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<ProductDto> Products, long TotalProducts, List<Category> Categories);

internal class GetProductsQueryHandler(IPublishEndpoint publishEndpoint, IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new CatalogBatchEvent(), cancellationToken);

        var pageNumber = query.PageNumber ?? 1;
        var pageSize = query.PageSize ?? 10;
        var skip = (pageNumber - 1) * pageSize;

        var batch = session.CreateBatchQuery();
        QueryStatistics stats;
        var filterProductBatch = batch.Query<Product>()
            .Where(p => p.Name != null && query.Name != null &&
                        p.Name.Contains(query.Name, StringComparison.OrdinalIgnoreCase));

        var pagingProductBatch = filterProductBatch
            .Stats(out stats).Skip(skip)
            .Take(pageSize);
        var orderProductBatch = query.SortType switch
        {
            1 => pagingProductBatch.OrderBy(p => p.DiscountedPrice),
            2 => pagingProductBatch.OrderByDescending(p => p.DiscountedPrice),
            3 => pagingProductBatch.OrderBy(p => p.Name),
            4 => pagingProductBatch.OrderByDescending(p => p.Name),
            _ => pagingProductBatch
        };
        var productBatch = orderProductBatch.ToList();
        var categoryBatch = batch.Query<Category>().ToList();
        await batch.Execute(cancellationToken);

        var productIds = productBatch.Result.Select(p => p.Id).ToList();
        var productImages = await session.Query<ProductImage>()
            .Where(img => productIds.Contains(img.ProductId))
            .ToListAsync(cancellationToken);
        var categoryDict = categoryBatch.Result.ToDictionary(c => c.Id);
        var productImagesDict = productImages
            .GroupBy(img => img.ProductId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var categoryFilter = new Category();
        if (query.Category != "") categoryFilter = categoryBatch.Result.FirstOrDefault(c => c.Name == query.Category);

        var productDtos = productBatch.Result
            .Where(p => categoryFilter != null && (query.Category == "" || p.CategoryId == categoryFilter.Id))
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DiscountedPrice = p.DiscountedPrice,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                Category = categoryDict.TryGetValue(p.CategoryId, out var category) ? category : null,
                ProductImages = productImagesDict.TryGetValue(p.Id, out var images)
                    ? images
                    : []
            });

        var categoryResult = categoryBatch.Result.ToList();

        var numberOfPage = stats.TotalResults;

        if (!string.IsNullOrEmpty(query.Category))
            numberOfPage = productBatch.Result.Count(p => categoryFilter != null && p.CategoryId == categoryFilter.Id);

        return new GetProductsResult(productDtos, numberOfPage, categoryResult);
    }
}