namespace Promotion.Application.Handlers.Coupons;

public record GetDiscountsQuery(
    int? PageNumber = 1,
    int? PageSize = 7)
    : IQuery<GetDiscountsResult>;

public record GetDiscountsResult(IEnumerable<DiscountDto> Discounts, long TotalDiscounts);

internal class GetDiscountsQueryHandler(IDocumentSession session)
    : IQueryHandler<GetDiscountsQuery, GetDiscountsResult>
{
    public async Task<GetDiscountsResult> Handle(GetDiscountsQuery query, CancellationToken cancellationToken)
    {
        var pageNumber = query.PageNumber ?? 1;
        var pageSize = query.PageSize ?? 10;
        var skip = (pageNumber - 1) * pageSize;

        var discounts = await session.Query<Discount>()
            .Stats(out var stats).Skip(skip)
            .Take(pageSize).ToListAsync(cancellationToken);

        var result = discounts.Select(
            d => new DiscountDto
            {
                Id = d.Id,
                Name = d.Name,
                PromotionType = d.PromotionType.ToString(),
                Amount = d.Amount,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                ProductIds = d.ProductIds,
                IsActive = d.IsActive
            }).ToList();

        return new GetDiscountsResult(result, stats.TotalResults);
    }
}