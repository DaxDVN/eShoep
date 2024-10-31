namespace Promotion.API.Coupons.UpdateCoupon;

public record UpdateCouponRequest(
    Guid Id,
    string Code,
    string Description,
    bool IsProductSpecific,
    string CouponType,
    int Amount,
    List<Guid> ProductId,
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