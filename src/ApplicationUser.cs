namespace CustomOnboardingProvider
{
    using Microsoft.AspNetCore.Identity;
    using IdentityServer4.Extensions;

    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; internal set; }
    }
}