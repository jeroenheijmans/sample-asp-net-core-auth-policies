using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;

namespace SampleAuthPolicies
{
    public static class AuthorizationPolicies
    {
        public static AuthorizationPolicyBuilder RequireNoBlacklistedUsers(this AuthorizationPolicyBuilder builder)
        {
            return builder
                .AddRequirements(new BlacklistUserRequirement("ABC-123", "XYZ-999"));
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

    public class AllowAnonymousBasedOnConfigRequirement : IAuthorizationRequirement
    {
        public bool IsAnonymousALlowed { get; set; }
    }

    public class AllowAnonymousBasedOnConfigHandler : AuthorizationHandler<AllowAnonymousBasedOnConfigRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AllowAnonymousBasedOnConfigRequirement requirement)
        {
            if (requirement.IsAnonymousALlowed)
            {
                context.Succeed(requirement);
            }
            else
            {
                // Based on: https://github.com/dotnet/aspnetcore/blob/master/src/Security/Authorization/Core/src/DenyAnonymousAuthorizationRequirement.cs
                var user = context.User;
                var userIsAnonymous =
                    user?.Identity == null ||
                    !user.Identities.Any(i => i.IsAuthenticated);
                if (!userIsAnonymous)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }

    public class BlacklistUserRequirement : IAuthorizationRequirement
    {
        public ISet<string> BlacklistedSubjectIds { get; set; }

        public BlacklistUserRequirement(params string[] blacklistedSubjectIds)
        {
            BlacklistedSubjectIds = new HashSet<string>(blacklistedSubjectIds);
        }
    }

    public class BlacklistUserHandler : AuthorizationHandler<BlacklistUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BlacklistUserRequirement requirement)
        {
            var subjectId = context?.User?.FindFirst(JwtClaimTypes.Subject)?.Value;

            if (!requirement.BlacklistedSubjectIds.Contains(subjectId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
