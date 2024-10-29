namespace Basket.API.Basket.StoreCart
{
    public record StoreCartRequest(Cart Cart);

    public record StoreCartResponse(string UserName);

    public class StoreCartEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreCartRequest request, ISender sender) =>
                {
                    var command = request.Adapt<StoreCartCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<StoreCartResponse>();

                    return Results.Created($"/basket/{response.UserName}", response);
                })
                .WithName("CreateCart")
                .Produces<StoreCartResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Cart")
                .WithDescription("Create Cart");
        }
    }
}