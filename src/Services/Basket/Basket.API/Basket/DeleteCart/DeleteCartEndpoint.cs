﻿namespace Basket.API.Basket.DeleteBasket
{
  public record DeleteCartResponse(bool IsSuccess);

  public class DeleteCartEndpoints : ICarterModule
  {
    public void AddRoutes(IEndpointRouteBuilder app)
    {
      app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
      {
        var result = await sender.Send(new DeleteCartCommand(userName));

        var response = result.Adapt<DeleteCartResponse>();

        return Results.Ok(response);
      })
      .WithName("DeleteCart")
      .Produces<DeleteCartResponse>(StatusCodes.Status200OK)
      .ProducesProblem(StatusCodes.Status400BadRequest)
      .ProducesProblem(StatusCodes.Status404NotFound)
      .WithSummary("Delete Cart")
      .WithDescription("Delete Cart");
    }
  }
}
