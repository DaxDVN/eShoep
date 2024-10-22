namespace Basket.API.Basket.CheckoutBasket
{
  public record CheckoutCartCommand() : ICommand<CheckoutCartResult>;
  public record CheckoutCartResult(bool IsSuccess);

  public class CheckoutCartCommandValidator
      : AbstractValidator<CheckoutCartCommand>
  {
    public CheckoutCartCommandValidator()
    {

    }
  }

  public class CheckoutCartCommandHandler
      ()
      : ICommandHandler<CheckoutCartCommand, CheckoutCartResult>
  {
    public async Task<CheckoutCartResult> Handle(CheckoutCartCommand command, CancellationToken cancellationToken)
    {
      return new CheckoutCartResult(true);
    }
  }
}
