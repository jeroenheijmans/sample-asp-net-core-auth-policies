using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
                   new TestUser { SubjectId = "ABC-123", Username = "john", Password = "secret", },
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
}
