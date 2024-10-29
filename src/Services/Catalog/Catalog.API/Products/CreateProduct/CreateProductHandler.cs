namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid CategoryId,
    List<string> ProductImages)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.ProductImages).NotEmpty().WithMessage("ImageFiles are required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = command.Adapt<Product>();
        product.CreatedAt = DateTime.UtcNow;

        session.Store(product);

        var productImages = command.ProductImages.Select(imageUrl => new ProductImage
        {
            ProductId = product.Id,
            ImageUrl = imageUrl,
            IsMain = false,
            CreatedAt = DateTime.UtcNow
        }).ToList();
        productImages.First().IsMain = true;
        session.Store<ProductImage>(productImages);

        await session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}