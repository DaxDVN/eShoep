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

  public class StoreCartCommandHandler(ICartRepository repository)
      : ICommandHandler<StoreCartCommand, StoreCartResult>
  {
    public async Task<StoreCartResult> Handle(StoreCartCommand command, CancellationToken cancellationToken)
    {
      await repository.StoreCart(command.Cart, cancellationToken);

      return new StoreCartResult(command.Cart.UserName);
    }
  }
}
