using Promotion.Grpc;

namespace Basket.API.Basket.StoreBasket
{
  public record StoreCartCommand(Cart Cart) : ICommand<StoreCartResult>;
  public record StoreCartResult(string UserName);

  public class StoreCartCommandValidator : AbstractValidator<StoreCartCommand>
  {
    public StoreCartCommandValidator()
    {
      RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
      RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
  }

  public class StoreCartCommandHandler(ICartRepository repository, CouponProtoService.CouponProtoServiceClient couponProto)
      : ICommandHandler<StoreCartCommand, StoreCartResult>
  {
    public async Task<StoreCartResult> Handle(StoreCartCommand command, CancellationToken cancellationToken)
    {

      try
      {
        foreach (var item in command.Cart.Items)
        {
          var coupon = await couponProto.GetCouponAsync(new GetCouponRequest { ProductId = item.ProductId.ToString() });
          if (coupon.CouponType == "FixedAmount")
          {
            item.Price -= coupon.Amount;
          }
          else
          {
            item.Price -= item.Price * coupon.Amount / 100;
          }
        }

        await repository.StoreCart(command.Cart, cancellationToken);

        return new StoreCartResult(command.Cart.UserName);

      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return new StoreCartResult(command.Cart.UserName);
      }
    }
  }
}
