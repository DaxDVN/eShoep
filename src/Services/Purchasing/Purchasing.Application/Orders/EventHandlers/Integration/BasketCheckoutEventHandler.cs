﻿using Common.Messaging.Events;
using MassTransit;
using Purchasing.Application.Orders.Commands.CreateOrder;
using Purchasing.Domain.Enums;

namespace Purchasing.Application.Orders.EventHandlers.Integration;

public class BasketCheckoutEventHandler(ISender sender, ILogger<BasketCheckoutEventHandler> logger)
    : IConsumer<CartCheckoutEvent>
{
    public async Task Consume(ConsumeContext<CartCheckoutEvent> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

        var command = MapToCreateOrderCommand(context.Message);
        await sender.Send(command);
    }

    private CreateOrderCommand MapToCreateOrderCommand(CartCheckoutEvent message)
    {
        // Create full order with incoming event data
        var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine,
            message.Country, message.State, message.ZipCode);
        var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV,
            message.PaymentMethod);
        var orderId = Guid.NewGuid();

        var listOrderItemDto =
            message.CartItems.Items.Select(item =>
                new OrderItemDto(orderId, item.ProductId, item.ProductName, item.Quantity, item.Price)).ToList();


        var orderDto = new OrderDto(
            orderId,
            message.CustomerId,
            message.CustomerName,
            addressDto,
            addressDto,
            paymentDto,
            OrderStatus.Pending,
            listOrderItemDto,
            message.TotalPrice);

        return new CreateOrderCommand(orderDto);
    }
}