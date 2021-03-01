namespace CustomOnboardingProvider
{
    using System.Collections.Generic;
    using IdentityServer4.Models;

    internal class OidcResources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResource(
                    name: "oauth",
                    userClaims: new[] { "sub" },
                    displayName: "Your user identifier")
            };
        }


    }
}