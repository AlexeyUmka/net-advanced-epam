using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityManagement;

public static class Config
{
    public const string CartingCatalogSystem = "carting-catalog-system";
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(CartingCatalogSystem),
            new ApiScope("offline_access")
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "manager",
                ClientName = "manager",

                RefreshTokenUsage = TokenUsage.ReUse,
                AllowOfflineAccess = true,
                AllowedGrantTypes = new List<string>() { GrantType.ClientCredentials },
                ClientSecrets = { new Secret("manager".Sha256()) },
                AllowedScopes = { CartingCatalogSystem, "offline_access",  },
                
                Claims = new List<ClientClaim>()
                {
                    new (JwtClaimTypes.Role, "manager"),
                },
                
                ClientClaimsPrefix = string.Empty
            },
            
            new Client
            {
                ClientId = "postman",
                ClientName = "postman",

                RefreshTokenUsage = TokenUsage.ReUse,
                AllowOfflineAccess = true,
                Enabled = true,
                AllowAccessTokensViaBrowser = true,
                AllowedGrantTypes = { GrantType.AuthorizationCode, GrantType.ClientCredentials },
                ClientSecrets = { new Secret("devsecret".Sha256()) },
                AllowedScopes = { CartingCatalogSystem, IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile },
                RedirectUris = new[] { "https://oauth.pstmn.io/v1/callback" },
                RequirePkce = false,

                Claims = new List<ClientClaim>()
                {
                    new (JwtClaimTypes.Role, "postman")
                },
            },
        };
}
