using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityManagement;

public static class Config
{
    private const string CreatePermission = "create";
    private const string ReadPermission = "read";
    private const string UpdatePermission = "update";
    private const string DeletePermission = "delete";
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    private static string[] DefaultUserClaims =
    {
        JwtClaimTypes.Role,
        JwtClaimTypes.Name
    };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(CreatePermission) {UserClaims = DefaultUserClaims },
            new ApiScope(ReadPermission) {UserClaims = DefaultUserClaims },
            new ApiScope(UpdatePermission) {UserClaims = DefaultUserClaims },
            new ApiScope(DeletePermission) {UserClaims = DefaultUserClaims },
            new ApiScope(IdentityServerConstants.StandardScopes.OfflineAccess),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "manager_app",
                ClientName = "manager_app",

                RefreshTokenUsage = TokenUsage.ReUse,
                AllowOfflineAccess = true,
                AllowAccessTokensViaBrowser = true,
                AllowedGrantTypes = { GrantType.AuthorizationCode },
                ClientSecrets = { new Secret("manager".Sha256()) },
                AllowedScopes = { CreatePermission, ReadPermission, UpdatePermission, DeletePermission, IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess },
                RedirectUris = new[] { "https://oauth.pstmn.io/v1/callback" },
                RequirePkce = false,
            },
            
            new Client
            {
                ClientId = "user_app",
                ClientName = "user_app",

                RefreshTokenUsage = TokenUsage.ReUse,
                AllowOfflineAccess = true,
                AllowAccessTokensViaBrowser = true,
                AllowedGrantTypes = { GrantType.AuthorizationCode },
                ClientSecrets = { new Secret("user".Sha256()) },
                AllowedScopes = { ReadPermission, IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess },
                RedirectUris = new[] { "https://oauth.pstmn.io/v1/callback" },
                RequirePkce = false,
            },
        };
}
