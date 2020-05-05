using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SampleAuthPolicies.Authorization
{
    public class AllowAnonymousBasedOnConfigRequirement : IAuthorizationRequirement
    {
        public bool IsAnonymousAllowed { get; set; }
    }

    public class AllowAnonymousBasedOnConfigHandler : AuthorizationHandler<AllowAnonymousBasedOnConfigRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AllowAnonymousBasedOnConfigRequirement requirement)
        {
            if (requirement.IsAnonymousAllowed)
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
}
