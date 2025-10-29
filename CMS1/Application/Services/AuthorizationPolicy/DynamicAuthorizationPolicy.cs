using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using UserManagement.Infrastructure;

namespace UserManagement.Services
{
    /// <summary>
    /// Custom policy provider that dynamically creates authorization policies
    /// based on permissions stored in the database.
    /// </summary>
    public class DynamicAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "AllPermissionPolicies";
        private const int CacheMinutes = 60; // Cache for 1 hour

        public DynamicAuthorizationPolicyProvider(
            IOptions<AuthorizationOptions> options,
            IServiceScopeFactory serviceScopeFactory,
            IMemoryCache cache)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _serviceScopeFactory = serviceScopeFactory;
            _cache = cache;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return _fallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return _fallbackPolicyProvider.GetFallbackPolicyAsync();
        }

        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // First, try to get from fallback (for built-in policies like "RequireEmployee")
            var policy = await _fallbackPolicyProvider.GetPolicyAsync(policyName);
            if (policy != null)
            {
                return policy;
            }

            // Try to get all permissions from cache
            var permissions = await _cache.GetOrCreateAsync(CacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CacheMinutes);
                return await LoadPermissionsFromDatabase();
            });

            // If the policy name matches a permission, create a policy for it
            if (permissions != null && permissions.Contains(policyName))
            {
                var policyBuilder = new AuthorizationPolicyBuilder();
                policyBuilder.RequireClaim("Permission", policyName);
                return policyBuilder.Build();
            }

            // Policy not found
            return null;
        }

        private async Task<HashSet<string>> LoadPermissionsFromDatabase()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<UMSDbContext>();

                var permissions = await Task.Run(() =>
                    dbContext.Permissions
                        .Select(p => p.SubModuleName)
                        .Distinct()
                        .ToHashSet()
                );

                return permissions;
            }
            catch (Exception ex)
            {
                // Log the error (inject ILogger if needed)
                Console.WriteLine($"Error loading permissions: {ex.Message}");
                return new HashSet<string>();
            }
        }

        /// <summary>
        /// Call this method to invalidate the cache when permissions are updated
        /// </summary>
        //public void InvalidateCache()
        //{
        //    _cache.Remove(CacheKey);
        //}
    }
}