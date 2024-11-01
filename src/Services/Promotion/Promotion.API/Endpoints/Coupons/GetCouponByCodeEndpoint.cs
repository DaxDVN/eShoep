using Promotion.Application.Handlers.Coupons;

namespace Promotion.API.Endpoints.Coupons;

public record GetCouponByCodeResponse(CouponDto Coupon);

public class GetCouponByCodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/promotion/coupons/{userId}/{code}", async (string userId, string code, ISender sender) =>
            {
                var result = await sender.Send(new GetCouponByCodeQuery(userId, code));

                var response = result.Adapt<GetCouponByCodeResponse>();

                return Results.Ok(response);
            })
            .WithName("GetCouponByCode")
            .Produces<GetCouponByCodeResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Coupon By Code")
            .WithDescription("Get Coupon By Code");
    }
}