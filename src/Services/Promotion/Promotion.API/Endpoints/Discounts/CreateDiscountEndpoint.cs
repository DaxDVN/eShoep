using Promotion.Application.Handlers.Discounts;

namespace Promotion.API.Endpoints.Discounts;

public record CreateDiscountRequest(
    string Name,
    string PromotionType,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate,
    List<Guid> ProductIds,
    bool IsActive);

public record CreateDiscountResponse(Guid Id);

public class CreateDiscountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/promotion/discounts",
                async (CreateDiscountRequest request, ISender sender) =>
                {
                    var command = request.Adapt<CreateDiscountCommand>();

                    var result = await sender.Send(command);

                    var response = result.Adapt<CreateDiscountResponse>();

                    return Results.Created($"/products/{response.Id}", response);
                })
            .WithName("CreateDiscount")
            .Produces<CreateDiscountResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Discount")
            .WithDescription("Create Discount");
    }
}