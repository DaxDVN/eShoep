namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid CategoryId,
    List<string> ProductImages)
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Product ID is required");

        RuleFor(command => command.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(2, 150).WithMessage("Name must be between 2 and 150 characters");

        RuleFor(command => command.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(command => command.CategoryId).NotEmpty().WithMessage("Category is required");

        RuleFor(command => command.ProductImages).NotEmpty().WithMessage("Image files are required");
    }
}

internal class UpdateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product is null) throw new ProductNotFoundException(command.Id);

        product.Name = command.Name;
        product.Description = command.Description;
        product.Price = command.Price;
        product.StockQuantity = command.StockQuantity;
        product.CategoryId = command.CategoryId;

        var oldImages = await session.Query<ProductImage>()
            .Where(img => img.ProductId == product.Id)
            .ToListAsync(cancellationToken);
        foreach (var image in oldImages) session.Delete(image);

        var productImages = command.ProductImages.Select(imageUrl => new ProductImage
        {
            ProductId = product.Id,
            ImageUrl = imageUrl,
            IsMain = false,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        if (productImages.Any()) productImages.First().IsMain = true;

        session.Store<ProductImage>(productImages);
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);
    }
}