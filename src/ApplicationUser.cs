namespace CustomOnboardingProvider
{
    using Microsoft.AspNetCore.Identity;
    using IdentityServer4.Extensions;
    internal class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string userName, string subjectId) : base(userName)
        {
            this.Id = userName;
            this.UserName = userName;
            this.SubjectId = subjectId;
        }

        public string EmployeeId { get; set; }
        public string SubjectId { get; internal set; }

    }
}