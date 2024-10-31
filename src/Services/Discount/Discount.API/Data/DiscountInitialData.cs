using Discount.API.Models;
using Marten;
using Marten.Schema;

namespace Discount.API.Data;

public class DiscountInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        // Check if any coupons exist; if not, seed the preconfigured coupons
        if (!await session.Query<Coupon>().AnyAsync())
        {
            var coupons = GetPreconfiguredCoupons();
            session.Store(coupons);
        }

        await session.SaveChangesAsync();
    }

    private static IEnumerable<Coupon> GetPreconfiguredCoupons()
    {
        return new List<Coupon>
        {
            new Coupon
            {
                Id = new Guid("1b4b5d6e-cf7e-4c18-a8f7-2b8c9c145b67"),
                ProductId = Guid.Parse("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                CouponType = CouponType.FixedAmount,
                Description = "IPhone Promotion",
                Amount = 150
            },
            new Coupon
            {
                Id = new Guid("b2a1b3c7-9d8f-4f5c-af72-b3f9c1e72e41"),
                ProductId = Guid.Parse("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
                CouponType = CouponType.Percentage,
                Description = "Samsung Promotion",
                Amount = 30
            }
        };
    }
}