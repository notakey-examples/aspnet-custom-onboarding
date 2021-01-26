namespace CustomOnboardingProvider
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using IdentityModel;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using IdentityServer4.Test;
    using Microsoft.AspNetCore.Identity;

    public class UserProfileService : IProfileService
    {

        private readonly IUserStore<ApplicationUser> _userManager;

        public UserProfileService(IUserStore<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var cancelToken = new CancellationToken();
            var user = await _userManager.FindByIdAsync(sub, cancelToken);
            // var principal = await _claimsFactory.CreateAsync(user);

            var claims = new List<Claim>();
            // claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            // Add custom claims in token here based on user properties or any other source
            claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            claims.Add(new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber));
            // claims.Add(new Claim(JwtClaimTypes.DisplayName, user.DisplayName));

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var cancelToken = new CancellationToken();
            var user = await _userManager.FindByIdAsync(sub, cancelToken);
            context.IsActive = user != null;
        }
    }
}