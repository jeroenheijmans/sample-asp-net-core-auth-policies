using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;

namespace SampleAuthPolicies
{
    public static class AuthorizationPolicies
    {
        public static AuthorizationPolicyBuilder RequireNoBlacklistedUsers(this AuthorizationPolicyBuilder builder)
        {
            builder.AddRequirements(new BlacklistUserRequirement("ABC-123", "XYZ-999"));
            return builder;
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
