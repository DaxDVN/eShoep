using Promotion.Application.Handlers.Discounts;

namespace Promotion.API.Endpoints.Discounts;

public record UpdateDiscountRequest(
    Guid Id,
    string Name,
    string PromotionType,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate,
    List<Guid> ProductIds,
    bool IsActive);

public record UpdateDiscountResponse(bool IsSuccess);

public class UpdateDiscountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/promotion/discounts",
                async (UpdateDiscountRequest request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateDiscountCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<UpdateDiscountResponse>();

                    return Results.Ok(response);
                })
            .WithName("UpdateDiscount")
            .Produces<UpdateDiscountResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Discount")
            .WithDescription("Update Discount");
    }
}