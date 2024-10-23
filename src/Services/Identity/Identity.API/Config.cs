using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Identity.API
{
  public static class Config
  {
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiResource> GetApis()
    {
      return new List<ApiResource>
            {
                new ApiResource("orders", "Orders Service"),
                new ApiResource("basket", "Basket Service"),
                new ApiResource("webhooks", "Webhooks registration Service"),
            };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
      return new List<ApiScope>
            {
                new ApiScope("orders", "Orders Service"),
                new ApiScope("basket", "Basket Service"),
                new ApiScope("webhooks", "Webhooks registration Service"),
            };
    }
    public static IEnumerable<Client> Clients(IConfiguration config) =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman",
                AllowedScopes = {"openid", "profile", "eShoep"},
                RedirectUris = {"https://www.getpostman.com/oauth2/callback"},
                ClientSecrets = new[] {new Secret("NotASecret".Sha256())},
                AllowedGrantTypes = {GrantType.ResourceOwnerPassword}
            },
            new Client
            {
                ClientId = "razorApp",
                ClientName = "Razor Page eCommerce App",
                ClientSecrets = {new Secret("razor_secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RedirectUris = { config["ClientApp"] + "/signin-oidc" },
                PostLogoutRedirectUris = { config["ClientApp"] + "/signout-callback-oidc" },
                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "eShoep" },
                AccessTokenLifetime = 3600 * 24 * 30, // 30 days
                AlwaysIncludeUserClaimsInIdToken = true
            },
            new Client
            {
                ClientId = "webapp",
                ClientName = "WebApp Client",
                ClientSecrets = new List<Secret>
                {
                    new Secret("secret".Sha256())
                },
                ClientUri = $"{config["WebAppClient"]}",
                AllowedGrantTypes = GrantTypes.Code,
                AllowAccessTokensViaBrowser = false,
                RequireConsent = false,
                AllowOfflineAccess = true,
                AlwaysIncludeUserClaimsInIdToken = true,
                RequirePkce = false,
                RedirectUris = new List<string>
                {
                    $"{config["WebAppClient"]}/signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>
                {
                    $"{config["WebAppClient"]}/signout-callback-oidc"
                },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    "orders",
                    "basket",
                    "webshoppingagg",
                    "webhooks"
                },
                AccessTokenLifetime = 60*60*2, // 2 hours
                IdentityTokenLifetime= 60*60*2 // 2 hours
            },
        };
  }
}
