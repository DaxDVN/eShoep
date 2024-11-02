using Basket.API.Repositories;

namespace Basket.API.Carts.DeleteCart;

public record DeleteCartCommand(string UserId) : ICommand<DeleteCartResult>;

public record DeleteCartResult(bool IsSuccess);

public class DeleteCartCommandValidator : AbstractValidator<DeleteCartCommand>
{
    public DeleteCartCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
    }
}

public class DeleteCartCommandHandler(IBasketRepository repository)
    : ICommandHandler<DeleteCartCommand, DeleteCartResult>
{
    public async Task<DeleteCartResult> Handle(DeleteCartCommand command, CancellationToken cancellationToken)
    {
        await repository.DeleteCart(command.UserId, cancellationToken);

        return new DeleteCartResult(true);
    }
}