namespace Basket.API.Models;

public class Cart
{
    public Cart(string userName)
    {
        UserName = userName;
    }

    //Required for Mapping
    public Cart()
    {
    }

    public string UserName { get; set; } = default!;
    public List<CartItem> Items { get; set; } = new();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
}