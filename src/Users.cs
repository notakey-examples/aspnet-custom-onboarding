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

    internal class Users
    {
        public static List<TestUser> Get()
        {
            return new List<TestUser> {
                new TestUser {
                    SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                    Username = "scott",
                    Password = "password",
                    Claims = new List<Claim> {
                        new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                        new Claim(JwtClaimTypes.Role, "admin")
                    }
                }
            };
        }
    }

    internal class UserProfileService : IProfileService
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

            // var claims = new ClaimsIdentity();
            // claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            // Add custom claims in token here based on user properties or any other source
            // claims.Add(new Claim("employee_id", user.EmployeeId ?? string.Empty));

            // context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var cancelToken = new CancellationToken();
            var user = await _userManager.FindByIdAsync(sub, cancelToken);
            context.IsActive = true;
            // throw new System.NotImplementedException();
        }
    }
}