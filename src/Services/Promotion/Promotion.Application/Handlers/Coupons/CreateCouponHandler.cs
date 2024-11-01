namespace Promotion.Application.Handlers.Coupons;

public record CreateCouponCommand(
    string Code,
    string PromotionType,
    string Description,
    int Amount,
    int MaxRedemptions,
    int RedemptionCount,
    DateTime ExpirationDate,
    List<Guid> UserIds,
    bool IsActive)
    : ICommand<CreateCouponResult>;

public record CreateCouponResult(Guid Id);

public class CreateCouponCommandValidator : AbstractValidator<CreateCouponCommand>
{
    public CreateCouponCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Coupon code is required.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Coupon code must be alphanumeric and uppercase.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.MaxRedemptions)
            .NotNull().WithMessage("MaxRedemptions must be specified.");

        RuleFor(x => x.PromotionType)
            .NotEmpty().WithMessage("Coupon type is required.")
            .IsInEnum().WithMessage("Coupon type is not valid.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive must be specified.");

        RuleFor(x => x.ExpirationDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Expiration date must be in the future.");
    }
}

internal class CreateCouponCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateCouponCommand, CreateCouponResult>
{
    public async Task<CreateCouponResult> Handle(CreateCouponCommand command, CancellationToken cancellationToken)
    {
        var coupon = command.Adapt<Coupon>();
        coupon.CreatedAt = DateTime.UtcNow;
        coupon.RedemptionCount = 0;
        session.Store(coupon);

        await session.SaveChangesAsync(cancellationToken);

        return new CreateCouponResult(coupon.Id);
    }
}