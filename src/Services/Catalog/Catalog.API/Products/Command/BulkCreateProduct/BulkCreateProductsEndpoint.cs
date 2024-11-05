using Catalog.API.Products.Command.CreateProduct;

namespace Catalog.API.Products.Command.BulkCreateProduct;
public record BulkCreateProductsRequest(
    List<CreateProductRequest> Products);

public record BulkCreateProductsResponse(
    List<Guid> ProductIds);

public class BulkCreateProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products/bulk",
                async (BulkCreateProductsRequest request, ISender sender) =>
                {
                    var commands = request.Products.Select(p => p.Adapt<CreateProductCommand>()).ToList();

                    var bulkCreateCommand = new BulkCreateProductsCommand(commands);

                    var result = await sender.Send(bulkCreateCommand);

                    var response = new BulkCreateProductsResponse(result.ProductIds);

                    return Results.Created("/products/bulk", response);
                })
            .WithName("BulkCreateProducts")
            .Produces<BulkCreateProductsResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Bulk Add Products")
            .WithDescription("Bulk Add multiple products at once");
    }
}