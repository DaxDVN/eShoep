using Refit;
using Shoep.Shop.Models.Auth;

namespace Shoep.Shop.Services;

public interface IIdentityService
{
    [Post("/api/auth/login")]
    Task<ApiResponse<string>> LoginAsync([Body] LoginRequest loginRequest);

    [Post("/api/auth/register")]
    Task<ApiResponse<string>> RegisterAsync([Body] RegisterRequest registerRequest);
}