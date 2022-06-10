using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace SnackBar.Services.Identity
{
    public static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>()
            {
                new ApiScope("snackbar","SnackBar Server"),
                 new ApiScope(name:"read",displayName:"read your data"),
                  new ApiScope(name:"write",displayName:"write your data"),
                   new ApiScope(name:"delete",displayName:"delete your data")
            };

        public static IEnumerable<Client> clients =>
            new List<Client>()
            {
                new Client()
                {
                    ClientId = "clients",
                    ClientSecrets = {new Secret("Secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"read", "write", "delete", "profile"},
                },
                new Client()
                {
                    ClientId = "snackbar",
                    ClientSecrets = {new Secret("Secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:44383/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44383/signout-callback-oidc" },
                    AllowedScopes = new List<string>()
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "snackbar"
                    },
                }
            };
    }
}
