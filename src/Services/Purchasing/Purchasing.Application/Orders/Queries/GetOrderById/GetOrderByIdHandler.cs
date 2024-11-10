using Mapster;

namespace Purchasing.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResult>
{
    public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery query,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == OrderId.Of(query.Id), cancellationToken);

        if (orders == null) throw new OrderNotFoundException(query.Id);
        var orderDto = new OrderDto(
            orders.Id.Value,
            orders.CustomerId.Value,
            orders.OrderName.Value,
            orders.ShippingAddress.Adapt<AddressDto>(),
            orders.BillingAddress.Adapt<AddressDto>(),
            orders.Payment.Adapt<PaymentDto>(), orders.Status,
            orders.OrderItems.Select(o =>
                new OrderItemDto(o.OrderId.Value, o.ProductId.Value, o.ProductName, o.Quantity, o.Price)).ToList(),
            orders.TotalPrice);

        return new GetOrderByIdResult(orderDto);
    }
}