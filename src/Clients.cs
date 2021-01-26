namespace CustomOnboardingProvider
{
    using System.Collections.Generic;
    using IdentityServer4.Models;

    internal class Clients
    {
        public static IEnumerable<IdentityServer4.Models.Client> Get()
        {
            return new List<Client>
            {
                // https://myauth.notakey.com/connect/authorize?
                    // client_id=testclient&
                    // redirect_uri=http%3A%2F%2Flocalhost%3A5002%2Fsignin-oidc&
                    // response_type=code&scope=openid%20profile&
                    // code_challenge=dDtbRNd50Kxs3SrxC5S6DQ0jmNepF78k-gR4k1iHEto&
                    // code_challenge_method=S256&
                    // nonce=637469155876569381.ZjcxNjA3MDgtMDNiMC00M2VlLTkxZjMtYTliNWUwODk3MWIzZDBlYmE2MGItMzdiNC00MTc1LWI4ZDktM2ZjN2E3OTU3MWI2&
                    // state=CfDJ8FIinetJNa9Hh-quTGFiNvsy85DME9tkJUyyI0RLib5mC1pm2su7q3XVsH4zQRPgI68mNFIknlhCrBz1vTguuGX7yw46r1L1V9u0PRgFJ7PJVyntjGZTQbH3tCMCmBApP16Xvzl7MR2pYJRejCwjpQdHxrLEaevqdm2DjmLSVkEx-0qtzytnDmLwWliMAC-_l1y6nuBoXocS1LImCcGzgKEuKQuzgpY3QGT5kSgKmvBmkHEif4dvZalTWhg4YRno0PshOh2Xzig4myjR8bcNVCU6FSRURAVs_R_VmfMPs7CmwdMjYtItB-IDmbVyFZYUkd7ByeXJcikO0fN1Bn5Bl61WubBm_Zcoc8SfOnHJCyuAqKl0w5aVz9z3cReGNXffTw&
                    // x-client-SKU=ID_NETSTANDARD2_0&
                    // x-client-ver=5.5.0.0
                new Client
                {
                    ClientId = "testclient",
                    ClientName = "Example client application using client credentials",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = new List<Secret> {new Secret("SuperSecretPassword".Sha256())}, // change me!
                    AllowedScopes = new List<string> { "email", "profile", "openid", "phone"},
                    RedirectUris = new List<string> {
                        "https://mbp15.notakey.com:5002/signin-oidc", // test client
                        "https://mynas.notakey.com/applications/f5f62469-8495-4b54-a3fd-d12b63e1c511/onboarding_requirements/OpenidOnboardingRequirement/openid_connect/callback"
                        },
                    RequirePkce = false
                }
            };
        }
    }
}