using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SampleAuthPolicies
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
               .AddInMemoryApiResources(new[] { new ApiResource("foo-api") })
               .AddInMemoryClients(new[]
               {
                    new Client
                    {
                        // Don't use RPO if you can prevent it. We use it here
                        // because it's the easiest way to demo with users.
                        ClientId = "legacy-rpo",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        AllowAccessTokensViaBrowser = false,
                        RequireClientSecret = false,
                        AllowedScopes = { "foo-api" },
                        
                    },
               })
               .AddTestUsers(new List<TestUser>
               {
                   new TestUser
                   {
                       SubjectId = "ABC-123",
                       Username = "john",
                       Password = "secret",
                       Claims = new[]
                       {
                           new Claim("role", "user"), // TODO: https://stackoverflow.com/q/61588752/419956
                           new Claim("domain", "foo") }, // TODO: https://stackoverflow.com/q/61588752/419956
                   },
                   new TestUser
                   {
                       SubjectId = "EFG-456",
                       Username = "mary",
                       Password = "secret",
                       Claims = new[]
                       {
                           new Claim("role", "user"), // TODO: https://stackoverflow.com/q/61588752/419956
                           new Claim("domain", "foo") }, // TODO: https://stackoverflow.com/q/61588752/419956
                   },
                   new TestUser
                   {
                       SubjectId = "HIJ-789",
                       Username = "admin",
                       Password = "secret",
                       Claims = new[]
                       {
                           new Claim("role", "admin"), // TODO: https://stackoverflow.com/q/61588752/419956
                       },
                   },
               })
               .AddDeveloperSigningCredential();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.ApiName = "foo-api";
                    options.SupportedTokens = SupportedTokens.Jwt;
                });

            services.AddSingleton<IAuthorizationHandler, BlacklistUserHandler>();
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireNoBlacklistedUsers()
                    .Build();
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseIdentityServer();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseFileServer();
            app.UseEndpoints(e => e.MapControllers().RequireAuthorization());
        }
    }
}
