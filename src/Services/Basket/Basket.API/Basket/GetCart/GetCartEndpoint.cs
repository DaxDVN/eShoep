namespace Basket.API.Basket.GetBasket
{
  public record GetCartResponse(Cart Cart);
  public class GetCartEndpoint : ICarterModule
  {
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      app.MapGet("/basket/{username}", async (string username, ISender sender) =>
      {
        if (string.IsNullOrEmpty(username))
        {
          throw new BadRequestException("Username not found");
        }

        var result = await sender.Send(new GetCartQuery(username));

        var response = result.Adapt<GetCartResponse>();

        return Results.Ok(response);
      })
          .WithName("GetCart")
          .Produces<GetCartResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Get Cart")
          .WithDescription("Get Cart"); ;
    }
  }
}
