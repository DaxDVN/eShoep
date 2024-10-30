namespace Basket.API.Carts.GetCart;

public record GetCartResponse(Cart Cart);

public class GetCartEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userId}", async (string userId, ISender sender) =>
            {
                if (string.IsNullOrEmpty(userId)) throw new BadRequestException("UserId not found");

                var result = await sender.Send(new GetCartQuery(userId));

                var response = result.Adapt<GetCartResponse>();

                return Results.Ok(response);
            })
            .WithName("GetCart")
            .Produces<GetCartResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Cart")
            .WithDescription("Get Cart");
        ;
    }
}