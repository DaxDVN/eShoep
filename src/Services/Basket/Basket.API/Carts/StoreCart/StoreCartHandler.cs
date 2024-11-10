using Basket.API.Repositories;
using Promotion.API;

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
    IBasketRepository repository)
    : ICommandHandler<StoreCartCommand, StoreCartResult>
{
    public async Task<StoreCartResult> Handle(StoreCartCommand command, CancellationToken cancellationToken)
    {
        await repository.StoreCart(command.Cart, cancellationToken);

        return new StoreCartResult(command.Cart.UserId);
    }
}