namespace Purchasing.Infrastructure.Data.Extensions;

internal class InitialData
{
    public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("e92573d3-e977-4cb4-ba18-620998e35c0a")), "customer1@myapp.com",
                "customer1@myapp.com"),
            Customer.Create(CustomerId.Of(new Guid("9c249d1a-1241-4f27-b2f4-5df60fe35864")), "customer2@myapp.com",
                "customer2@myapp.com")
        };

    public static IEnumerable<Product> Products =>
        new List<Product>
        {
            Product.Create(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")), "IPhone X", 500),
            Product.Create(ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), "Samsung 10", 400),
            Product.Create(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), "Huawei Plus", 650),
            Product.Create(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), "Xiaomi Mi", 450)
        };

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address1 = Address.Of("", "", "customer1@myapp.com", "Bahcelievler No:4", "Turkey",
                "Istanbul", "38050");
            var address2 = Address.Of("", "", "customer2@myapp.com", "Broadway No:1", "England", "Nottingham",
                "08050");

            var payment1 = Payment.Of("customer1", "5555555555554444", "12/28", "355", 1);
            var payment2 = Payment.Of("customer2", "8885555555554444", "06/30", "222", 1);

            var order1 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("e92573d3-e977-4cb4-ba18-620998e35c0a")),
                OrderName.Of("customer1@myapp.com"),
                address1,
                address1,
                payment1);
            order1.Add(ProductId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")), 2, 500);
            order1.Add(ProductId.Of(new Guid("c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914")), 1, 400);

            var order2 = Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("9c249d1a-1241-4f27-b2f4-5df60fe35864")),
                OrderName.Of("ORD_2"),
                address2,
                address2,
                payment2);
            order2.Add(ProductId.Of(new Guid("4f136e9f-ff8c-4c1f-9a33-d12f689bdab8")), 1, 650);
            order2.Add(ProductId.Of(new Guid("6ec1297b-ec0a-4aa1-be25-6726e3b51a27")), 2, 450);

            return new List<Order> { order1, order2 };
        }
    }
}