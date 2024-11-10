namespace Purchasing.Application.Orders.Commands.UpdateStatusOrder;

public class UpdateOrderStatusHandler(IApplicationDbContext dbContext)
    : ICommandHandler<UpdateOrderStatusCommand, UpdateOrderStatusResult>
{
    public async Task<UpdateOrderStatusResult> Handle(UpdateOrderStatusCommand command,
        CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.OrderId);
        var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);

        if (order is null) throw new OrderNotFoundException(command.OrderId);

        order.UpdateStatus(command.NewStatus);

        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderStatusResult(true);
    }
}