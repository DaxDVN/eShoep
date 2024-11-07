using Promotion.Application.Handlers.Coupons;

namespace Promotion.API.Endpoints.Coupons;

public record UpdateCouponRequest(
    Guid Id,
    string Code,
    string Description,
    string PromotionType,
    int Amount,
    int MaxRedemptions,
    int RedemptionCount,
    List<Guid> UserIds,
    bool IsActive,
    DateTime ExpirationDate);

public record UpdateCouponResponse(bool IsSuccess);

public class UpdateCouponEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/promotion/coupons",
                async (UpdateCouponRequest request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateCouponCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<UpdateCouponResponse>();

                    return Results.Ok(response);
                })
            .WithName("UpdateCoupon")
            .Produces<UpdateCouponResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Coupon")
            .WithDescription("Update Coupon");
    }
}