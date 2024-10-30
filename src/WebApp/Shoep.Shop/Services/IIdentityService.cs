using Refit;
using Shoep.Shop.Models.Auth;

namespace Shoep.Shop.Services;

public interface IIdentityService
{
    [Post("/login")]
    Task<ApiResponse<string>> LoginAsync([Body] LoginRequest loginRequest);
}