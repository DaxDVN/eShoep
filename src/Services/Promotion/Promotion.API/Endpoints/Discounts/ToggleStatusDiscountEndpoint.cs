using Promotion.Application.Handlers.Discounts;

namespace Promotion.API.Endpoints.Discounts;

public record ToggleStatusDiscountRequest(Guid Id, bool IsActive);

public record ToggleStatusDiscountResponse(bool IsSuccess);

public class ToggleStatusDiscountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/promotion/discounts/toggle-status",
                async (ToggleStatusDiscountRequest request, ISender sender) =>
                {
                    var command = request.Adapt<ToggleStatusDiscountCommand>();
                    var result = await sender.Send(command);
                    var response = new ToggleStatusDiscountResponse(result.IsSuccess);

                    return Results.Ok(response);
                })
            .WithName("ToggleStatusDiscount")
            .Produces<ToggleStatusDiscountResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Toggle Discount Status")
            .WithDescription("Toggle the active status of a coupon.");
    }
}