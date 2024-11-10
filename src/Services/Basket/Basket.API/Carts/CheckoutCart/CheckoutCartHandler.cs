using Basket.API.Dtos;
using Basket.API.Repositories;
using Common.Messaging.Events;
using MassTransit;
using Promotion.API;

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

public class CheckoutCartCommandHandler(
    IBasketRepository basketRepository,
    CouponProtoService.CouponProtoServiceClient couponProto,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutCartCommand, CheckoutCartResult>
{
    public async Task<CheckoutCartResult> Handle(CheckoutCartCommand command, CancellationToken cancellationToken)
    {
        var cartCheckout = command.CartCheckoutDto;

        var basket = await basketRepository.GetCart(cartCheckout.CustomerId.ToString(), cancellationToken);
        if (basket == null) return new CheckoutCartResult(false);


        if (cartCheckout.CouponApply != string.Empty)
        {
            cartCheckout = await ApplyCouponAsync(cartCheckout);
        }

        var eventMessage = cartCheckout.Adapt<CartCheckoutEvent>();
        eventMessage.CartItems = new CartItemsCheckout(basket.Items.Select(item => new CartItemCheckout
        {
            Price = item.Price,
            ProductName = item.ProductName,
            Quantity = item.Quantity,
            ProductId = item.ProductId
        }).ToList());
        await publishEndpoint.Publish(eventMessage, cancellationToken);

        await basketRepository.DeleteCart(cartCheckout.CustomerId.ToString(), cancellationToken);

        return new CheckoutCartResult(true);
    }

    private async Task<CartCheckoutDto> ApplyCouponAsync(CartCheckoutDto cartCheckout)
    {
        var coupon = await couponProto.ApplyCouponAsync(new ApplyCouponRequest
        {
            UserId = cartCheckout.CustomerId.ToString(),
            CouponCode = cartCheckout.CouponApply
        });

        if (coupon is null) return cartCheckout;

        decimal discountAmount;

        if (coupon.PromotionType == "FixedAmount")
        {
            discountAmount = coupon.Amount;
        }
        else
        {
            discountAmount = cartCheckout.TotalPrice * coupon.Amount / 100;
        }

        var maxDiscount = cartCheckout.TotalPrice * 0.3m;
        if (discountAmount > maxDiscount)
        {
            discountAmount = maxDiscount;
        }

        cartCheckout.TotalPrice -= discountAmount;

        return cartCheckout;
    }

}