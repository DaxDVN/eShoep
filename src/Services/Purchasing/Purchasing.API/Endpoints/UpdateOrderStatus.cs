using Purchasing.Application.Orders.Commands.UpdateStatusOrder;
using Purchasing.Domain.Enums;

namespace Purchasing.API.Endpoints;

public record UpdateOrderStatusRequest(Guid OrderId, string NewStatus);

public record UpdateOrderStatusResponse(bool IsSuccess);

public class UpdateOrderStatus : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders/status", async (UpdateOrderStatusRequest request, ISender sender) =>
            {
                var command = new UpdateOrderStatusCommand(request.OrderId, Enum.Parse<OrderStatus>(request.NewStatus));

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateOrderStatusResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateOrderStatus")
            .Produces<UpdateOrderStatusResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Order Status")
            .WithDescription("Updates the status of an existing order");
    }
}