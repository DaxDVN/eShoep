using Shoep.Management.Enums;

namespace Shoep.Management.Models.Purchasing;

public class Order
{
    private readonly List<OrderItem> _orderItems = new();
    public Guid Id { get; set; }
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();

    public Guid CustomerId { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public decimal TotalPrice
    {
        get => OrderItems.Sum(x => x.Price * x.Quantity);
        private set { }
    }
}