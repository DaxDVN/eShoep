namespace Promotion.API.Coupons.ToggleStatusCoupon;

public record ToggleStatusCouponRequest(Guid Id, bool IsActive);

public record ToggleStatusCouponResponse(bool IsSuccess);

public class ToggleStatusCouponEndpoint
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/coupons/toggle-status",
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