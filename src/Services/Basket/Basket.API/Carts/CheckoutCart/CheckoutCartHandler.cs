﻿using Basket.API.Dtos;
using Common.Messaging.Events;
using MassTransit;

namespace Basket.API.Carts.CheckoutCart;

public record CheckoutCartCommand(CartCheckoutDto CartCheckoutDto) : ICommand<CheckoutCartResult>;

public record CheckoutCartResult(bool IsSuccess);

public class CheckoutCartCommandValidator
    : AbstractValidator<CheckoutCartCommand>
{
    public CheckoutCartCommandValidator()
    {
        RuleFor(x => x.CartCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.CartCheckoutDto.CustomerName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CheckoutCartCommandHandler(ICartRepository cartRepository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutCartCommand, CheckoutCartResult>
{
    public async Task<CheckoutCartResult> Handle(CheckoutCartCommand command, CancellationToken cancellationToken)
    {
        var basket = await cartRepository.GetCart(command.CartCheckoutDto.CustomerId.ToString(), cancellationToken);
        if (basket == null) return new CheckoutCartResult(false);

        var eventMessage = command.CartCheckoutDto.Adapt<CartCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await publishEndpoint.Publish(eventMessage, cancellationToken);

        await cartRepository.DeleteCart(command.CartCheckoutDto.CustomerName, cancellationToken);

        return new CheckoutCartResult(true);
    }
}