using System.Net;
using Refit;
using Shoep.Web.Models.Basket;

namespace Shoep.Web.Services;

public interface IBasketService
{
    [Get("/basket-service/basket/{userName}")]
    Task<GetCartResponse> GetBasket(string userName);

    [Post("/basket-service/basket")]
    Task<StoreCartResponse> StoreBasket(StoreCartRequest request);

    [Delete("/basket-service/basket/{userName}")]
    Task<DeleteCartResponse> DeleteBasket(string userName);

    [Post("/basket-service/basket/checkout")]
    Task<CheckoutCartResponse> CheckoutBasket(CheckoutCartRequest request);

    public async Task<CartModel> LoadUserBasket()
    {
        var userName = "swn";
        CartModel basket;

        try
        {
            var getBasketResponse = await GetBasket(userName);
            basket = getBasketResponse.Cart;
        }
        catch (ApiException apiException) when (apiException.StatusCode == HttpStatusCode.NotFound)
        {
            basket = new CartModel
            {
                UserName = userName,
                Items = []
            };
        }

        return basket;
    }
}