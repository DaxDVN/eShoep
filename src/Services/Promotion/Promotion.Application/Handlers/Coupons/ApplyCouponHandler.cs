namespace Promotion.Application.Handlers.Coupons;

public record ApplyCouponCommand(Guid UserId, Guid CouponId) : ICommand<ApplyCouponResult>;

public record ApplyCouponResult(CouponDto Coupon);

public class ApplyCouponHandler(IDocumentSession session) : ICommandHandler<ApplyCouponCommand, ApplyCouponResult>
{
    public async Task<ApplyCouponResult> Handle(ApplyCouponCommand command, CancellationToken cancellationToken)
    {
        var coupon = await session.LoadAsync<Coupon>(command.CouponId, cancellationToken);

        if (coupon is null) throw new CouponNotFoundException(command.CouponId);

        coupon.RedemptionCount++;

        session.Update(coupon);
        var couponUsage = new CouponUsage
        {
            CouponId = coupon.Id,
            UserId = command.UserId,
            Id =  Guid.NewGuid(),
            UsedAt = DateTime.Now
        };

        session.Store(couponUsage);
        await session.SaveChangesAsync(cancellationToken);

        var result = coupon.Adapt<CouponDto>();
        return new ApplyCouponResult(result);
    }
}