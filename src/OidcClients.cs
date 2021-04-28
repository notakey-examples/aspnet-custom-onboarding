namespace CustomOnboardingProvider
{
    using System;
    using System.Collections.Generic;
    using IdentityServer4.Models;

    internal class OidcClients
    {
        public static IEnumerable<IdentityServer4.Models.Client> Get()
        {
            var clientId = Environment.GetEnvironmentVariable("OIDC_CLIENT_ID");
            var clientName = Environment.GetEnvironmentVariable("OIDC_CLIENT_NAME");
            var clientSecret = Environment.GetEnvironmentVariable("OIDC_CLIENT_SECRET");
            var clientRedirectUri = Environment.GetEnvironmentVariable("OIDC_CLIENT_REDIRECT_URI");

            return new List<Client>
            {
                new Client
                {
                    ClientId = clientId,
                    ClientName = clientName,
                    // AllowedGrantTypes = GrantTypes.Code,
                    AllowedGrantTypes = GrantTypes.Implicit,
                    // AlwaysIncludeUserClaimsInIdToken = true,
                    ClientSecrets = new List<Secret> {new Secret(clientSecret.Sha256())},
                    AllowedScopes = new List<string> { "email", "profile", "openid", "phone"},
                    RedirectUris = new List<string> { clientRedirectUri },
                    RequirePkce = false
                }
            };
        }
    }
}