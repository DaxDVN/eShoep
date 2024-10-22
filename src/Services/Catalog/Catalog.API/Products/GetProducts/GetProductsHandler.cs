using Marten.Linq;

namespace Catalog.API.Products.GetProducts
{
  // Query nhận PageNumber và PageSize để hỗ trợ phân trang
  public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 3) : IQuery<GetProductsResult>;

  public record GetProductsResult(IEnumerable<ProductDto> ProductDtos, long TotalProducts);

  internal class GetProductsQueryHandler(IDocumentSession session) : IQueryHandler<GetProductsQuery, GetProductsResult>
  {
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
      var pageNumber = query.PageNumber ?? 1;
      var pageSize = query.PageSize ?? 10;
      var skip = (pageNumber - 1) * pageSize;

      var batch = session.CreateBatchQuery();

      QueryStatistics stats;
      var productBatch = batch.Query<Product>()
                              .Stats(out stats)
                              .Skip(skip)
                              .Take(pageSize)
                              .ToList();

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


      var productDtos = productBatch.Result.Select(p => new ProductDto
      {
        Id = p.Id,
        Name = p.Name,
        CategoryId = p.CategoryId,
        Category = categoryDict.TryGetValue(p.CategoryId, out var category) ? category : null,
        ProductImages = productImagesDict.TryGetValue(p.Id, out var images) ? images : new List<ProductImage>()
      });

      return new GetProductsResult(productDtos, stats.TotalResults);
    }
  }
}
