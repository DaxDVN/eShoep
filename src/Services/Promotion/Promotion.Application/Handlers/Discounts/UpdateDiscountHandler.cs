namespace Promotion.Application.Handlers.Discounts;

public record UpdateDiscountCommand(
    Guid Id,
    string Name,
    string PromotionType,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate,
    List<Guid> ProductIds,
    bool IsActive)
    : ICommand<UpdateDiscountResult>;

public record UpdateDiscountResult(bool IsSuccess);

public class UpdateDiscountCommandValidator : AbstractValidator<UpdateDiscountCommand>
{
    public UpdateDiscountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Discount Name is required.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Discount Name must be alphanumeric and uppercase.");

        RuleFor(x => x.PromotionType)
            .NotEmpty().WithMessage("PromotionType is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.ProductIds)
            .NotNull().WithMessage("Product ID list cannot be null.");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive must be specified.");

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("StartDate must be in the future.");

        RuleFor(x => x.EndDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("EndDate must be in the future.");
    }
}

internal class UpdateDiscountCommandHandler(IDocumentSession session)
    : ICommandHandler<UpdateDiscountCommand, UpdateDiscountResult>
{
    public async Task<UpdateDiscountResult> Handle(UpdateDiscountCommand command, CancellationToken cancellationToken)
    {
        var discount = await session.LoadAsync<Discount>(command.Id, cancellationToken);

        if (discount is null) throw new DiscountNotFoundException(command.Id);

        discount.Name = command.Name;
        discount.PromotionType = Enum.Parse<PromotionType>(command.PromotionType);
        discount.Amount = command.Amount;
        discount.StartDate = command.StartDate;
        discount.EndDate = command.EndDate;
        discount.ProductIds = command.ProductIds;
        discount.IsActive = command.IsActive;

        session.Update(discount);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateDiscountResult(true);
    }
}