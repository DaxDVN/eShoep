namespace Purchasing.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        await CheckCustomer(command.Order, cancellationToken);

        await CheckProducts(command.Order, cancellationToken);

        var order = CreateNewOrder(command.Order);

        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateOrderResult(order.Id.Value);
    }

    private async Task CheckProducts(OrderDto commandOrder, CancellationToken cancellationToken)
    {
        var productIds = commandOrder.OrderItems.Select(orderItem => ProductId.Of(orderItem.ProductId)).ToList();

        var existingProductIds = await dbContext.Products
            .Where(product => productIds.Contains(product.Id))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);

        var missingProductIds = productIds.Except(existingProductIds).ToList();

        var productMissing = commandOrder.OrderItems
            .Where(orderItem => missingProductIds.Contains(ProductId.Of(orderItem.ProductId)))
            .Select(orderItem => Product.Create(
                ProductId.Of(orderItem.ProductId),
                orderItem.ProductName,
                orderItem.Price))
            .ToList();

        if (productMissing.Any())
        {
            await dbContext.Products.AddRangeAsync(productMissing, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }


    private async Task CheckCustomer(OrderDto commandOrder, CancellationToken cancellationToken)
    {
        var customerId = CustomerId.Of(commandOrder.CustomerId);
        var customer = await dbContext.Customers.AsNoTracking().AnyAsync(c => c.Id == customerId, cancellationToken);
        if (customer == false)
        {
            var newCustomer = Customer.Create(customerId, commandOrder.OrderName,
                commandOrder.BillingAddress.EmailAddress);
            await dbContext.Customers.AddAsync(newCustomer, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of(
            orderDto.ShippingAddress.FirstName,
            orderDto.ShippingAddress.LastName,
            orderDto.ShippingAddress.EmailAddress,
            orderDto.ShippingAddress.AddressLine,
            orderDto.ShippingAddress.Country,
            orderDto.ShippingAddress.State,
            orderDto.ShippingAddress.ZipCode
        );

        var billingAddress = Address.Of(
            orderDto.BillingAddress.FirstName,
            orderDto.BillingAddress.LastName,
            orderDto.BillingAddress.EmailAddress,
            orderDto.BillingAddress.AddressLine,
            orderDto.BillingAddress.Country,
            orderDto.BillingAddress.State,
            orderDto.BillingAddress.ZipCode
        );

        var newOrder = Order.Create(
            OrderId.Of(Guid.NewGuid()),
            CustomerId.Of(orderDto.CustomerId),
            OrderName.Of(orderDto.OrderName),
            shippingAddress,
            billingAddress,
            Payment.Of(
                orderDto.Payment.CardName,
                orderDto.Payment.CardNumber,
                orderDto.Payment.Expiration,
                orderDto.Payment.Cvv,
                orderDto.Payment.PaymentMethod
            )
        );

        foreach (var orderItemDto in orderDto.OrderItems)
        {
            newOrder.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
        }

        return newOrder;
    }
}