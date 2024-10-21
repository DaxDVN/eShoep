namespace Catalog.API.Products.GetProducts
{
  public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
  public record GetProductsResult(IEnumerable<ProductDto> ProductDtos);

  internal class GetProductsQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
  {
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
      var products = await session.Query<Product>()
          .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

      var result = products.Adapt<IEnumerable<ProductDto>>();
      return new GetProductsResult(result);
    }
  }
}
