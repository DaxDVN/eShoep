namespace Shoep.Management.Models.Purchasing;

public class OrderItem
{
    internal OrderItem(Guid orderId, Guid productId, int quantity, decimal price)
    {
        OrderItemId = Guid.NewGuid();
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }

    public Guid OrderItemId { get; set; }

    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
}