namespace Promotion.Application.Handlers.Discounts;

public record ToggleStatusDiscountCommand(
    Guid Id,
    bool IsActive)
    : ICommand<ToggleStatusDiscountResult>;

public record ToggleStatusDiscountResult(bool IsSuccess);

internal class ToggleStatusDiscountCommandHandler(IDocumentSession session)
    : ICommandHandler<ToggleStatusDiscountCommand, ToggleStatusDiscountResult>
{
    public async Task<ToggleStatusDiscountResult> Handle(ToggleStatusDiscountCommand command,
        CancellationToken cancellationToken)
    {
        var discount = await session.LoadAsync<Discount>(command.Id, cancellationToken);

        if (discount is null) throw new DiscountNotFoundException(command.Id);

        discount.IsActive = command.IsActive;

        session.Update(discount);
        await session.SaveChangesAsync(cancellationToken);

        return new ToggleStatusDiscountResult(true);
    }
}