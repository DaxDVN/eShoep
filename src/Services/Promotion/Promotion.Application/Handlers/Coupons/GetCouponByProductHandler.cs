namespace Promotion.Application.Handlers.Coupons;

public record GetCouponByProductQuery(Guid ProductId) : IQuery<GetCouponByProductResult>;

public record GetCouponByProductResult(CouponDto Coupon);

internal class GetCouponByProductQueryHandler(IDocumentSession session)
    : IQueryHandler<GetCouponByProductQuery, GetCouponByProductResult>
{
    public async Task<GetCouponByProductResult> Handle(GetCouponByProductQuery query, CancellationToken cancellationToken)
    {
        var test = await session.Query<Coupon>().ToListAsync(token: cancellationToken);
        var coupon = await session.Query<Coupon>()
            .FirstOrDefaultAsync(c => c.ProductIds.Contains(query.ProductId), cancellationToken);

        if (coupon is null)
            throw new CouponNotFoundException(query.ProductId);

        var couponDto = coupon.Adapt<CouponDto>();
        return new GetCouponByProductResult(couponDto);
    }
}