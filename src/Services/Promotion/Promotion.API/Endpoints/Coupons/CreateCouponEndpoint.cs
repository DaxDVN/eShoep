using Promotion.Application.Handlers.Coupons;

namespace Promotion.API.Endpoints.Coupons;

public record CreateCouponRequest(
    string Code,
    string Description,
    string PromotionType,
    int Amount,
    int MaxRedemptions,
    int RedemptionCount,
    List<Guid> UserIds,
    bool IsActive,
    DateTime ExpirationDate);

public record CreateCouponResponse(Guid Id);

public class CreateCouponEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/promotion/coupons",
                async (CreateCouponRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateCouponCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<CreateCouponResponse>();

                    return Results.Created($"/products/{response.Id}", response);
                })
            .WithName("CreateCoupon")
            .Produces<CreateCouponResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Coupon")
            .WithDescription("Create Coupon");
    }
}