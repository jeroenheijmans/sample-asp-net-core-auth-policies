using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace SampleAuthPolicies
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("foo-api")
            {
                Scopes =
                {
                    new Scope("foo-api.with.roles", new[] { "role" }),
                }
            }
        };

        public static IEnumerable<Client> Clients => new[]
        {
            new Client
            {
                // Don't use RPO if you can prevent it. We use it here
                // because it's the easiest way to demo with users.
                ClientId = "legacy-rpo",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowAccessTokensViaBrowser = false,
                RequireClientSecret = false,
                AllowedScopes = { "foo-api", "foo-api.with.roles" },
            },
        };

        public static List<TestUser> TestUsers => new List<TestUser>
               {
                   new TestUser
                   {
                       SubjectId = "ABC-123",
                       Username = "john",
                       Password = "secret",
                       Claims = { new Claim("role", "user") },
                   },
                   new TestUser
                   {
                       SubjectId = "ABC-556",
                       Username = "marcus",
                       Password = "secret",
                       Claims = { new Claim("role", "user") },
                   },
                   new TestUser
                   {
                       SubjectId = "EFG-456",
                       Username = "mary",
                       Password = "secret",
                       Claims = { new Claim("role", "editor") },
                   },
                   new TestUser
                   {
                       SubjectId = "HIJ-789",
                       Username = "admin",
                       Password = "secret",
                       Claims = { new Claim("role", "admin") },
                   },
               };
    }
}
