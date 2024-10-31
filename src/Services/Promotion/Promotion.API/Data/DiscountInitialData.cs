using Marten.Schema;

namespace Promotion.API.Data;

public class PromotionInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

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
            new()
            {
                Id = new Guid("1b4b5d6e-cf7e-4c18-a8f7-2b8c9c145b67"),
                Code = "IPHONE150",
                ProductId = new List<Guid>
                {
                    Guid.Parse("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                    Guid.Parse("12345678-1234-1234-1234-123456789012")
                },
                CouponType = CouponType.FixedAmount,
                IsProductSpecific = true,
                Description = "Giảm 150k cho iPhone",
                Amount = 150,
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddDays(30)
            },
            new()
            {
                Id = new Guid("b2a1b3c7-9d8f-4f5c-af72-b3f9c1e72e41"),
                Code = "SAMSUNG30",
                ProductId = new List<Guid> { Guid.Parse("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914") },
                CouponType = CouponType.Percentage,
                IsProductSpecific = true,
                Description = "Giảm 30% cho Samsung",
                Amount = 30,
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddDays(15)
            },
            new()
            {
                Id = new Guid("d1f5a6a2-90c2-49c4-b1f7-8e3a56934c7e"),
                Code = "SUMMER20",
                ProductId = new List<Guid>(),
                CouponType = CouponType.Percentage,
                IsProductSpecific = false,
                Description = "Giảm giá 20% cho toàn bộ đơn hàng",
                Amount = 20,
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddMonths(1)
            },
            new()
            {
                Id = new Guid("e2b1c4d8-4f88-4ed6-8c0b-43c7ecff59c9"),
                Code = "CLEARANCE50",
                ProductId = new List<Guid>(),
                CouponType = CouponType.FixedAmount,
                IsProductSpecific = false,
                Description = "Giảm 50k cho toàn bộ hóa đơn",
                Amount = 50,
                IsActive = true,
                ExpirationDate = DateTime.UtcNow.AddDays(10)
            }
        };
    }
}