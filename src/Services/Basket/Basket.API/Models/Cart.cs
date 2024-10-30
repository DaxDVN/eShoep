namespace Basket.API.Models;

public class Cart
{
    public Cart(string userId)
    {
        UserId = userId;
    }

    //Required for Mapping
    public Cart()
    {
    }

    public string UserId { get; set; } = default!;
    public List<CartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
}