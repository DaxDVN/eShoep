namespace Basket.API.Exceptions;

public class CartNotFoundException : NotFoundException
{
    public CartNotFoundException(string userId) : base("Cart", userId)
    {
    }
}