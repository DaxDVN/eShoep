using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();

        if (!await session.Query<Category>().AnyAsync()) session.Store(GetPreconfiguredCategories());

        // var products = GetPreconfiguredProducts();
        //
        // if (!await session.Query<Product>().AnyAsync()) session.Store(products);
        //
        // if (!await session.Query<ProductImage>().AnyAsync())
        //     foreach (var product in products)
        //     {
        //         var images = GetPreconfiguredProductImages(product.Id);
        //         session.Store<ProductImage>(images);
        //     }

        await session.SaveChangesAsync(cancellation);
    }

    private static IEnumerable<Category> GetPreconfiguredCategories()
    {
        return new List<Category>
        {
            new()
            {
                Id = new Guid("d738d8b9-866a-41b8-9b48-bf3b0cc0f9b4"),
                Name = "Smartphones",
                Description = "All kinds of smartphones, including iOS, Android, and more."
            },
            new()
            {
                Id = new Guid("ba758b7b-9e8f-457d-ae6f-7b6b7a4129c8"),
                Name = "Cameras",
                Description = "Digital and DSLR cameras for photography and videography."
            },
            new()
            {
                Id = new Guid("b3f19f7e-5f07-4bfc-bdb8-63b6b2a8b7c9"),
                Name = "Tablets",
                Description = "All types of tablets including iPads and Android tablets."
            },
            new()
            {
                Id = new Guid("e7543b7e-cf7a-4d18-8d2f-b258c9c67579"),
                Name = "Laptops",
                Description = "Various brands and models of laptops for personal and professional use."
            },
            new()
            {
                Id = new Guid("a55f3f6e-88d9-4e76-b1f9-2b5b4d9e4e5c"),
                Name = "Headphones",
                Description = "Wireless and wired headphones for all audio needs."
            },
            new()
            {
                Id = new Guid("b85d7b21-3f0d-4d32-982a-601bf56c1f67"),
                Name = "Smartwatches",
                Description = "Wearable technology including Apple Watch, Samsung Galaxy Watch, and more."
            },
            new()
            {
                Id = new Guid("c56f8b62-2fd1-4b6b-a5f8-4e6a9e5d7f29"),
                Name = "Gaming Consoles",
                Description = "Popular gaming consoles like PlayStation, Xbox, and Nintendo Switch."
            },
            new()
            {
                Id = new Guid("db239f8a-8b7f-4e3f-b8d2-2f5c5d6a7f98"),
                Name = "Home Appliances",
                Description = "Essential home appliances like refrigerators, washing machines, and microwaves."
            },
            new()
            {
                Id = new Guid("e6549d2a-5a8e-4d9a-9a4f-8f2b7b9e4d5a"),
                Name = "Speakers",
                Description = "Portable and home speakers with high-quality sound."
            },
            new()
            {
                Id = new Guid("f76d9a7b-9f2c-4b8d-8e9b-4b5c3f7e6a7d"),
                Name = "Wearable Accessories",
                Description = "Accessories for smartwatches, fitness bands, and other wearables."
            }
        };
    }


    // private static IEnumerable<Product> GetPreconfiguredProducts()
    // {
    //     return new List<Product>
    //     {
    //         new()
    //         {
    //             Id = new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61"),
    //             Name = "IPhone X",
    //             Description =
    //                 "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
    //             Price = 950.00M,
    //             StockQuantity = 10,
    //             CategoryId = new Guid("d738d8b9-866a-41b8-9b48-bf3b0cc0f9b4"), // Smartphones
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new()
    //         {
    //             Id = new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914"),
    //             Name = "Samsung 10",
    //             Description =
    //                 "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
    //             Price = 840.00M,
    //             StockQuantity = 10,
    //             CategoryId = new Guid("d738d8b9-866a-41b8-9b48-bf3b0cc0f9b4"), // Smartphones
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new()
    //         {
    //             Id = new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8"),
    //             Name = "Huawei Plus",
    //             Description =
    //                 "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
    //             Price = 650.00M,
    //             StockQuantity = 10,
    //             CategoryId = new Guid("d738d8b9-866a-41b8-9b48-bf3b0cc0f9b4"), // Smartphones
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new()
    //         {
    //             Id = new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27"),
    //             Name = "Xiaomi Mi 9",
    //             Description =
    //                 "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
    //             Price = 470.00M,
    //             StockQuantity = 10,
    //             CategoryId = new Guid("d738d8b9-866a-41b8-9b48-bf3b0cc0f9b4"), // Smartphones
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new()
    //         {
    //             Id = new Guid("b786103d-c621-4f5a-b498-23452610f88c"),
    //             Name = "HTC U11+ Plus",
    //             Description =
    //                 "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
    //             Price = 380.00M,
    //             StockQuantity = 10,
    //             CategoryId = new Guid("d738d8b9-866a-41b8-9b48-bf3b0cc0f9b4"), // Smartphones
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new()
    //         {
    //             Id = new Guid("c4bbc4a2-4555-45d8-97cc-2a99b2167bff"),
    //             Name = "LG G7 ThinQ",
    //             Description =
    //                 "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
    //             Price = 240.00M,
    //             StockQuantity = 10,
    //             CategoryId = new Guid("d738d8b9-866a-41b8-9b48-bf3b0cc0f9b4"), // Smartphones
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new()
    //         {
    //             Id = new Guid("93170c85-7795-489c-8e8f-7dcf3b4f4188"),
    //             Name = "Panasonic Lumix",
    //             Description =
    //                 "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
    //             Price = 240.00M,
    //             StockQuantity = 10,
    //             CategoryId = new Guid("ba758b7b-9e8f-457d-ae6f-7b6b7a4129c8"), // Cameras
    //             CreatedAt = DateTime.UtcNow
    //         }
    //     };
    // }

    // private static List<ProductImage> GetPreconfiguredProductImages(Guid productId)
    // {
    //     return new List<ProductImage>
    //     {
    //         new()
    //         {
    //             Id = Guid.NewGuid(),
    //             ProductId = productId,
    //             ImageUrl = "https://example.com/images/" + productId + "/1.jpg",
    //             IsMain = true,
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new()
    //         {
    //             Id = Guid.NewGuid(),
    //             ProductId = productId,
    //             ImageUrl = "https://example.com/images/" + productId + "/2.jpg",
    //             IsMain = false,
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new()
    //         {
    //             Id = Guid.NewGuid(),
    //             ProductId = productId,
    //             ImageUrl = "https://example.com/images/" + productId + "/3.jpg",
    //             IsMain = false,
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new()
    //         {
    //             Id = Guid.NewGuid(),
    //             ProductId = productId,
    //             ImageUrl = "https://example.com/images/" + productId + "/4.jpg",
    //             IsMain = false,
    //             CreatedAt = DateTime.UtcNow
    //         },
    //         new()
    //         {
    //             Id = Guid.NewGuid(),
    //             ProductId = productId,
    //             ImageUrl = "https://example.com/images/" + productId + "/5.jpg",
    //             IsMain = false,
    //             CreatedAt = DateTime.UtcNow
    //         }
    //     };
    // }
}