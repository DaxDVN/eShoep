namespace Promotion.Application.Handlers.Coupons;

public record ApplyCouponCommand(Guid UserId, string CouponCode) : ICommand<ApplyCouponResult>;

public record ApplyCouponResult(CouponDto Coupon);

public class ApplyCouponHandler(IDocumentSession session) : ICommandHandler<ApplyCouponCommand, ApplyCouponResult>
{
    public async Task<ApplyCouponResult> Handle(ApplyCouponCommand command, CancellationToken cancellationToken)
    {
        var coupon = await session.Query<Coupon>()
            .FirstOrDefaultAsync(c => c.Code == command.CouponCode, cancellationToken);

        if (coupon is null) throw new CouponNotFoundException(Guid.Empty);

        coupon.RedemptionCount++;

        session.Update(coupon);
        var couponUsage = new CouponUsage
        {
            CouponId = coupon.Id,
            UserId = command.UserId,
            Id = Guid.NewGuid(),
            UsedAt = DateTime.Now
        };

        session.Store(couponUsage);
        await session.SaveChangesAsync(cancellationToken);

        var result = coupon.Adapt<CouponDto>();
        return new ApplyCouponResult(result);
    }
}