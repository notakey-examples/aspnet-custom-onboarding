namespace CustomOnboardingProvider
{
    using System.Threading.Tasks;
    using IdentityServer4.Validation;
    internal class TransientResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}