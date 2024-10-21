namespace Catalog.API.Products.GetProductById
{
  public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
  public record GetProductByIdResult(ProductDto ProductDto);

  internal class GetProductByIdQueryHandler
      (IDocumentSession session)
      : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
  {
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
      var product = await session.LoadAsync<Product>(query.Id, cancellationToken);

      if (product is null)
      {
        throw new ProductNotFoundException(query.Id);
      }

      var result = product.Adapt<ProductDto>();

      return new GetProductByIdResult(result);
    }
  }
}
