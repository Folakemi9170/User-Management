using Microsoft.AspNetCore.Authorization;
using UserManagement.Services;


namespace UserManagement.Application.Services.AuthorizationPolicy
{
    public static class PermissionPolicyService
    {
        /// <summary>
        /// Registers the dynamic authorization policy provider for database-driven policies
        /// </summary>
        public static IServiceCollection AddDynamicPolicies(this IServiceCollection services)
        {
            // Register the custom policy provider as a singleton
            services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();

            // Add basic authorization with static policies
            services.AddAuthorization(options =>
            {
                // Static policy that all employees must satisfy
                options.AddPolicy("RequireEmployee", policy =>
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == System.Security.Claims.ClaimTypes.Role)));
            });

            return services;
        }
    }
}