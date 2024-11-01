namespace Promotion.Application.Handlers.Coupons;

public record GetCouponsQuery(
    int? PageNumber = 1,
    int? PageSize = 3,
    int? SortType = 1)
    : IQuery<GetCouponsResult>;

public record GetCouponsResult(IEnumerable<CouponDto> Coupons, long TotalCoupons);

internal class GetCouponsQueryHandler(IDocumentSession session)
    : IQueryHandler<GetCouponsQuery, GetCouponsResult>
{
    public async Task<GetCouponsResult> Handle(GetCouponsQuery query, CancellationToken cancellationToken)
    {
        var pageNumber = query.PageNumber ?? 1;
        var pageSize = query.PageSize ?? 10;
        var skip = (pageNumber - 1) * pageSize;

        var coupons = await session.Query<Coupon>()
            .Stats(out var stats).Skip(skip)
            .Take(pageSize).ToListAsync(cancellationToken);

        var result = coupons.Select(
            c => c.Adapt<CouponDto>()).ToList();

        return new GetCouponsResult(result, stats.TotalResults);
    }
}