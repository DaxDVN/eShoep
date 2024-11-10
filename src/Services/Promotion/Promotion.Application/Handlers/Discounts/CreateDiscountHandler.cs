using Common.Messaging.Events;
using MassTransit;

namespace Promotion.Application.Handlers.Discounts;

public record CreateDiscountCommand(
    string Name,
    string PromotionType,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate,
    List<Guid> ProductIds,
    bool IsActive)
    : ICommand<CreateDiscountResult>;

public record CreateDiscountResult(Guid Id);

public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
{
    public CreateDiscountCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Discount Name is required.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Discount Name must be alphanumeric and uppercase.");

        RuleFor(x => x.PromotionType)
            .NotEmpty().WithMessage("PromotionType is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive must be specified.");

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("StartDate must be in the future.");

        RuleFor(x => x.EndDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("EndDate must be in the future.");
    }
}

internal class CreateDiscountCommandHandler(IDocumentSession session, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CreateDiscountCommand, CreateDiscountResult>
{
    public async Task<CreateDiscountResult> Handle(CreateDiscountCommand command, CancellationToken cancellationToken)
    {
        var discount = command.Adapt<Discount>();
        session.Store(discount);
        await session.SaveChangesAsync(cancellationToken);

        var eventMessage =
            new DiscountCreatedEvent
            {
                DiscountId = discount.Id,
                Amount = discount.Amount,
                PromotionType = discount.PromotionType.ToString(),
                ProductIds = discount.ProductIds,
                ExpirationDate = discount.EndDate
            };
        await publishEndpoint.Publish(eventMessage, cancellationToken);

        return new CreateDiscountResult(discount.Id);
    }
}