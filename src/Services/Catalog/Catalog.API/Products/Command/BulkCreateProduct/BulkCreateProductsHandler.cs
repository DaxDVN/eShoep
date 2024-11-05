using Catalog.API.Products.Command.CreateProduct;

namespace Catalog.API.Products.Command.BulkCreateProduct;

public record BulkCreateProductsCommand(
    List<CreateProductCommand> Products)
    : ICommand<BulkCreateProductsResult>;

public record BulkCreateProductsResult(
    List<Guid> ProductIds);

internal class BulkCreateProductsHandler(IDocumentSession session)
    : ICommandHandler<BulkCreateProductsCommand, BulkCreateProductsResult>
{
    public async Task<BulkCreateProductsResult> Handle(BulkCreateProductsCommand command, CancellationToken cancellationToken)
    {
        var productIds = new List<Guid>();

        foreach (var createProductCommand in command.Products)
        {
            var product = createProductCommand.Adapt<Product>();
            product.CreatedAt = DateTime.UtcNow;

            session.Store(product);
            productIds.Add(product.Id);

            var productImages = createProductCommand.ProductImages.Select(imageUrl => new ProductImage
            {
                ProductId = product.Id,
                ImageUrl = imageUrl,
                IsMain = false,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            if (productImages.Any())
                productImages.First().IsMain = true;

            session.Store<ProductImage>(productImages);
        }

        await session.SaveChangesAsync(cancellationToken);

        return new BulkCreateProductsResult(productIds);
    }
}
