using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SampleAuthPolicies.Authorization;

namespace SampleAuthPolicies
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
               .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
               .AddInMemoryClients(IdentityServerConfig.Clients)
               .AddTestUsers(IdentityServerConfig.TestUsers)
               .AddDeveloperSigningCredential();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.ApiName = "foo-api";
                });

            services.AddSingleton<IAuthorizationHandler, BlacklistUsersHandler>();
            services.AddSingleton<IAuthorizationHandler, AllowAnonymousBasedOnConfigHandler>();
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireNoBlacklistedUsers()
                    .Build();

                options.AddPolicy(nameof(AuthorizationPolicies.AtLeastEditor), AuthorizationPolicies.AtLeastEditor());
                options.AddPolicy(nameof(AuthorizationPolicies.AllowAnonymousBasedOnConfig), AuthorizationPolicies.AllowAnonymousBasedOnConfig());
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
