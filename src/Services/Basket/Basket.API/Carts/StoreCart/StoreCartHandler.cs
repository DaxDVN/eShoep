using Promotion.Grpc;

namespace Basket.API.Carts.StoreCart;

public record StoreCartCommand(Cart Cart) : ICommand<StoreCartResult>;

public record StoreCartResult(string UserId);

public class StoreCartCommandValidator : AbstractValidator<StoreCartCommand>
{
    public StoreCartCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserId).NotEmpty().WithMessage("UserId is required");
    }
}

public class StoreCartCommandHandler(
    ICartRepository repository,
    CouponProtoService.CouponProtoServiceClient couponProto)
    : ICommandHandler<StoreCartCommand, StoreCartResult>
{
    public async Task<StoreCartResult> Handle(StoreCartCommand command, CancellationToken cancellationToken)
    {
        await DeductPromotion(command.Cart, cancellationToken);

        await repository.StoreCart(command.Cart, cancellationToken);

        return new StoreCartResult(command.Cart.UserId);
    }

    private async Task DeductPromotion(Cart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await couponProto.GetCouponAsync(new GetCouponRequest
                { ProductId = item.ProductId.ToString() });
            if (coupon.CouponType == "FixedAmount")
                item.Price -= coupon.Amount;
            else
                item.Price -= item.Price * coupon.Amount / 100;
        }
    }
}