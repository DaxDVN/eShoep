using Promotion.Application.Handlers.Coupons;

namespace Promotion.API.Endpoints.Coupons;

public record ToggleStatusCouponRequest(Guid Id, bool IsActive);

public record ToggleStatusCouponResponse(bool IsSuccess);

public class ToggleStatusCouponEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/promotion/coupons/toggle-status",
                async (ToggleStatusCouponRequest request, ISender sender) =>
                {
                    var command = request.Adapt<ToggleStatusCouponCommand>();
                    var result = await sender.Send(command);
                    var response = new ToggleStatusCouponResponse(result.IsSuccess);

                    return Results.Ok(response);
                })
            .WithName("ToggleStatusCoupon")
            .Produces<ToggleStatusCouponResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Toggle Coupon Status")
            .WithDescription("Toggle the active status of a coupon.");
    }
}