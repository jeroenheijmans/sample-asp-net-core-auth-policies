using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
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
                           new Claim("role", "user"),
                           new Claim("domain", "foo") },
                   },
                   new TestUser
                   {
                       SubjectId = "QWE-567",
                       Username = "admin",
                       Password = "secret",
                       Claims = new[]
                       {
                           new Claim("role", "admin"),
                       },
                   },
               })
               .AddDeveloperSigningCredential();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.Audience = "foo-api";
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
            app.UseEndpoints(e => e.MapControllers());
        }
    }

    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var role = context.Subject.FindFirst(ClaimTypes.Role);
            context.IssuedClaims.Add(role);
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
