using Microsoft.AspNetCore.Authorization;

namespace SampleAuthPolicies.Authorization
{
    public static class AuthorizationPolicies
    {
        public static AuthorizationPolicyBuilder RequireNoBlacklistedUsers(this AuthorizationPolicyBuilder builder)
        {
            return builder
                .AddRequirements(new BlacklistUsersRequirement("ABC-123", "XYZ-999"));
        }

        public static AuthorizationPolicy AtLeastEditor()
        {
            return new AuthorizationPolicyBuilder()
                .RequireRole("editor", "admin") // but not "user"
                .Build();
        }

        public static AuthorizationPolicy AllowAnonymousBasedOnConfig()
        {
            return new AuthorizationPolicyBuilder()
                .AddRequirements(new AllowAnonymousBasedOnConfigRequirement
                {
                    IsAnonymousALlowed = true, // Could come from IConfiguration...
                })
                .Build();
        }
    }
}
