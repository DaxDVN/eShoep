namespace Promotion.Application.Handlers.Coupons;

public record UpdateCouponCommand(
    Guid Id,
    string Code,
    string Description,
    string CouponType,
    int Amount,
    bool IsActive,
    DateTime ExpirationDate)
    : ICommand<UpdateCouponResult>;

public record UpdateCouponResult(bool IsSuccess);

public class UpdateCouponCommandValidator : AbstractValidator<UpdateCouponCommand>
{
    public UpdateCouponCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Coupon ID is required");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Coupon code is required.")
            .Matches(@"^[A-Z0-9]+$").WithMessage("Coupon code must be alphanumeric and uppercase.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.CouponType)
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

internal class UpdateCouponCommandHandler(IDocumentSession session)
    : ICommandHandler<UpdateCouponCommand, UpdateCouponResult>
{
    public async Task<UpdateCouponResult> Handle(UpdateCouponCommand command, CancellationToken cancellationToken)
    {
        var coupon = await session.LoadAsync<Coupon>(command.Id, cancellationToken);

        if (coupon is null) throw new CouponNotFoundException(command.Id);

        coupon.Code = command.Code;
        coupon.Description = command.Description;
        coupon.CouponType = Enum.Parse<CouponType>(command.CouponType);
        coupon.Amount = command.Amount;
        coupon.IsActive = command.IsActive;
        coupon.ExpirationDate = command.ExpirationDate;


        session.Update(coupon);
        await session.SaveChangesAsync(cancellationToken);

        return new UpdateCouponResult(true);
    }
}