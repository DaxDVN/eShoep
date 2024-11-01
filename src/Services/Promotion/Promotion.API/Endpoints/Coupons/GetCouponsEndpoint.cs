using Promotion.Application.Handlers.Coupons;

namespace Promotion.API.Endpoints.Coupons;

public record GetCouponsRequest(
    int? PageNumber = 1,
    int? PageSize = 3,
    int? SortType = 1);

public record GetCouponsResponse(IEnumerable<CouponDto> Coupons, long TotalCoupons);

public class GetCouponsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/promotion/coupons", async ([AsParameters] GetCouponsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetCouponsQuery>();

                var result = await sender.Send(query);

                var response = result.Adapt<GetCouponsResponse>();

                return Results.Ok(response);
            })
            .WithName("GetCoupons")
            .Produces<GetCouponsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Coupons")
            .WithDescription("Get Coupons");
    }
}