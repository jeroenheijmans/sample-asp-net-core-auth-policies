using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;

namespace SampleAuthPolicies.Authorization
{
    public class BlacklistUsersRequirement : IAuthorizationRequirement
    {
        public ISet<string> BlacklistedSubjectIds { get; set; }

        public BlacklistUsersRequirement(params string[] blacklistedSubjectIds)
        {
            BlacklistedSubjectIds = new HashSet<string>(blacklistedSubjectIds);
        }
    }

    public class BlacklistUsersHandler : AuthorizationHandler<BlacklistUsersRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BlacklistUsersRequirement requirement)
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
