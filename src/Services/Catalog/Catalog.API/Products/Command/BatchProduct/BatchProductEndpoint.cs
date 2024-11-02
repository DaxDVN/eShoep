namespace Catalog.API.Products.Command.BatchProduct;

public record BatchProductResponse(string Status);

public class BatchProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/batch", async (ISender sender) =>
            {
                var result = await sender.Send(new BatchProductCommand());
                var response = result.Adapt<BatchProductResponse>();
                return Results.Ok(response);
            })
            .WithName("BatchProduct")
            .Produces<BatchProductResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Batch Product")
            .WithDescription("Batch Product");
    }
}