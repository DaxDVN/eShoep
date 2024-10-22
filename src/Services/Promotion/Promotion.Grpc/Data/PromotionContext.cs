using Microsoft.EntityFrameworkCore;
using Promotion.Grpc.Models;

namespace Promotion.Grpc.Data;

public class PromotionContext : DbContext
{
  public DbSet<Coupon> Coupons { get; set; } = default!;

  public PromotionContext(DbContextOptions<PromotionContext> options)
  : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Coupon>().HasData(
        new Coupon { Id = 1, Description = "IPhone Promotion", Amount = 150 },
        new Coupon { Id = 2, Description = "Samsung Promotion", Amount = 100 }
        );
  }
}