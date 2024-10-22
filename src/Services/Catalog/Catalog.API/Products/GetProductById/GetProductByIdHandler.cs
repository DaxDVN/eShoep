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
      var batch = session.CreateBatchQuery();

      var productBatch = batch.Load<Product>(query.Id);


      var productImagesBatch = batch.Query<ProductImage>().Where(image => image.ProductId == query.Id).ToList();

      await batch.Execute(cancellationToken);

      if (productBatch.Result is null)
      {
        throw new ProductNotFoundException(query.Id);
      }

      var category = await session.Query<Category>().FirstOrDefaultAsync(c => c.Id == productBatch.Result.CategoryId, cancellationToken);

      var productDto = productBatch.Result.Adapt<ProductDto>();

      productDto.ProductImages = (List<ProductImage>)productImagesBatch.Result;
      productDto.Category = category;

      return new GetProductByIdResult(productDto);
    }
  }
}
