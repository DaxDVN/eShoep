namespace Promotion.API.Coupons.ToggleStatusCoupon;

public record ToggleStatusCouponCommand(Guid Id, bool IsActive) : ICommand<ToggleStatusCouponResult>;

public record ToggleStatusCouponResult(bool IsSuccess);

internal class ToggleStatusCouponHandler(IDocumentSession session)
    : ICommandHandler<ToggleStatusCouponCommand, ToggleStatusCouponResult>
{
    public async Task<ToggleStatusCouponResult> Handle(ToggleStatusCouponCommand command, CancellationToken cancellationToken)
    {
        var coupon = await session.LoadAsync<Coupon>(command.Id, cancellationToken);

        if (coupon is null) throw new CouponNotFoundException(command.Id);

        coupon.IsActive = command.IsActive;

        session.Update(coupon);
        await session.SaveChangesAsync(cancellationToken);

        return new ToggleStatusCouponResult(true);
    }
}