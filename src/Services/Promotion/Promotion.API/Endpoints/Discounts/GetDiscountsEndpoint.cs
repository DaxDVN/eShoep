using Promotion.Application.Handlers.Coupons;
using Promotion.Application.Handlers.Discounts;

namespace Promotion.API.Endpoints.Discounts;

public record GetDiscountsRequest(
    int? PageNumber = 1,
    int? PageSize = 7);

public record GetDiscountsResponse(IEnumerable<DiscountDto> Discounts, long TotalDiscounts);

public class GetDiscountsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/promotion/discounts", async ([AsParameters] GetDiscountsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetDiscountsQuery>();

                var result = await sender.Send(query);

                var response = result.Adapt<GetDiscountsResponse>();

                return Results.Ok(response);
            })
            .WithName("GetDiscounts")
            .Produces<GetDiscountsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Discounts")
            .WithDescription("Get a paginated list of discounts.");
    }
}