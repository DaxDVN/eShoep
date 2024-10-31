namespace Promotion.API.Coupons.CreateCoupon;

public record CreateCouponCommand(
    string Code,
    string Description,
    bool IsProductSpecific,
    string CouponType,
    int Amount,
    List<Guid> ProductId,
    bool IsActive,
    DateTime ExpirationDate)
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

        RuleFor(x => x.IsProductSpecific)
            .NotNull().WithMessage("IsProductSpecific must be specified.");

        RuleFor(x => x.CouponType)
            .NotEmpty().WithMessage("Coupon type is required.")
            .IsInEnum().WithMessage("Coupon type is not valid.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.ProductId)
            .NotNull().WithMessage("Product ID list cannot be null.")
            .When(x => x.IsProductSpecific) // Only validate if IsProductSpecific is true
            .Must(x => x.Count > 0).WithMessage("At least one product ID must be provided when the coupon is product-specific.");

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
        var product = command.Adapt<Coupon>();
        product.CreatedAt = DateTime.UtcNow;

        session.Store(product);

        await session.SaveChangesAsync(cancellationToken);

        return new CreateCouponResult(product.Id);
    }
}