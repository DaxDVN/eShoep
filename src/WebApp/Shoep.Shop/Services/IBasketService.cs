using System.Net;
using Refit;
using Shoep.Shop.Models.Auth;
using Shoep.Shop.Models.Basket;

namespace Shoep.Shop.Services;

public interface IBasketService
{
    [Get("/basket-service/basket/{userId}")]
    Task<GetCartResponse> GetBasket(string userId);

    [Post("/basket-service/basket")]
    Task<StoreCartResponse> StoreBasket(StoreCartRequest request);

    [Delete("/basket-service/basket/{userId}")]
    Task<DeleteCartResponse> DeleteBasket(string userId);

    [Post("/basket-service/basket/checkout")]
    Task<CheckoutCartResponse> CheckoutBasket(CheckoutCartRequest request);

    public async Task<CartModel> LoadUserBasket(string userId = "swn")
    {
        CartModel basket;

        try
        {
            var getBasketResponse = await GetBasket(userId);
            basket = getBasketResponse.Cart;
        }
        catch (ApiException apiException) when (apiException.StatusCode == HttpStatusCode.NotFound)
        {
            basket = new CartModel
            {
                UserId = userId,
                Items = []
            };
        }

        return basket;
    }
}