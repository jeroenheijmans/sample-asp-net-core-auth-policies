using System;
using Microsoft.AspNetCore.Authorization;

namespace SampleAuthPolicies
{
    public static class AuthorizationPolicies
    {
        public static Action<AuthorizationPolicyBuilder> RequireAuthenticatedUser
            => builder => builder.RequireAuthenticatedUser();
    }
}
