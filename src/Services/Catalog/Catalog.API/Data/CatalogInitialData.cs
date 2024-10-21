using Marten.Schema;

namespace Catalog.API.Data
{
  public class CatalogInitialData : IInitialData
  {
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
      using var session = store.LightweightSession();

      if (await session.Query<Product>().AnyAsync())
        return;

      session.Store<Product>(GetPreconfiguredProducts());
      await session.SaveChangesAsync();
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>()
            {
                new Product()
                {
                    Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
                    Name = "IPhone X",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Price = 950.00M,
                    StockQuantity = 10,
                    CategoryId = new Guid("5332k396-8457-4cf0-815c-ed2117c4ff61"),
                    CreatedAt = DateTime.UtcNow,
                },
                new Product()
                {
                    Id = new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
                    Name = "Samsung 10",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Price = 840.00M,
                    StockQuantity = 10,
                    CategoryId = new Guid("53323996-8457-4cf0-815c-ed2b77c22261"),
                    CreatedAt = DateTime.UtcNow,
                },
                new Product()
                {
                    Id = new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
                    Name = "Huawei Plus",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Price = 650.00M,
                    StockQuantity = 10,
                    CategoryId = new Guid("53312196-8457-4cf0-815c-ed2b11222261"),
                    CreatedAt = DateTime.UtcNow,
                },
                new Product()
                {
                    Id = new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
                    Name = "Xiaomi Mi 9",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Price = 470.00M,
                    StockQuantity = 10,
                    CategoryId = new Guid("11212196-8457-4cf0-815c-e93811222261"),
                    CreatedAt = DateTime.UtcNow,
                },
                new Product()
                {
                    Id = new Guid("b786103d-c621-4f5a-b498-23452610f88c"),
                    Name = "HTC U11+ Plus",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Price = 380.00M,
                    StockQuantity = 10,
                    CategoryId = new Guid("11000196-8457-4cf0-815c-e93811000261"),
                    CreatedAt = DateTime.UtcNow,
                },
                new Product()
                {
                    Id = new Guid("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"),
                    Name = "LG G7 ThinQ",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Price = 240.00M,
                    StockQuantity = 10,
                    CategoryId = new Guid("11aaa196-8457-4cf0-815c-e9381aaa0261"),
                    CreatedAt = DateTime.UtcNow,
                },
                new Product()
                {
                    Id = new Guid("93170c85-7795-489c-8e8f-7dcf3b4f4188"),
                    Name = "Panasonic Lumix",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Price = 240.00M,
                    StockQuantity = 10,
                    CategoryId = new Guid("11aa8826-8457-4cf0-815c-e938kdj90261"),
                    CreatedAt = DateTime.UtcNow,
                }
            };

  }
}
