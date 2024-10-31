using Microsoft.EntityFrameworkCore;
using Promotion.API.Models;

namespace Promotion.API.Data;

public class PromotionContext : DbContext
{
    public PromotionContext(DbContextOptions<PromotionContext> options)
        : base(options)
    {
    }

    public DbSet<Coupon> Coupons { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon
            {
                Id = 1,
                ProductId = Guid.Parse("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                CouponType = CouponType.FixedAmount,
                Description = "IPhone Promotion",
                Amount = 150
            },
            new Coupon
            {
                Id = 2,
                ProductId = Guid.Parse("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
                CouponType = CouponType.Percentage,
                Description = "Samsung Promotion",
                Amount = 30
            }
        );
    }

    public void ClearCoupons()
    {
        Database.ExecuteSqlRaw("DELETE FROM Coupons;");
        SaveChanges();
    }

    public void ResetDatabase()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
}