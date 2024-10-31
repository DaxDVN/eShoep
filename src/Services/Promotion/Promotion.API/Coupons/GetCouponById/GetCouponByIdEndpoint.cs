using Promotion.API.Dtos;

namespace Promotion.API.Coupons.GetCouponById;

public record GetCouponByIdResponse(CouponDto Coupon);

public class GetCouponByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/promotion/coupons/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetCouponByIdQuery(id));

                var response = result.Adapt<GetCouponByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetCouponById")
            .Produces<GetCouponByIdResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Coupon By Id")
            .WithDescription("Get Coupon By Id");
    }
}