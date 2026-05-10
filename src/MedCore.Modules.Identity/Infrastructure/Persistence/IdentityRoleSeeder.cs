namespace MedCore.Modules.Identity.Infrastructure.Persistence;

using MedCore.Common.Authorization;
using MedCore.Modules.Identity.Domain.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


internal static class IdentityRoleSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(nameof(IdentityRoleSeeder));

        var roles = new Dictionary<string, string>
        {
            [AppRoles.Admin] = "System Administrator",
            [AppRoles.Doctor] = "Doctor User",
            [AppRoles.Patient] = "Patient User"
        };
        
        RoleSeederLogMessages.RoleSeedingStarted(logger, string.Join(", ", roles.Keys), null);
        
        foreach (var role in roles)
        {
            var exists = await roleManager.RoleExistsAsync(role.Key);
            if (exists)
            {
                RoleSeederLogMessages.RoleAlreadyExists(logger, role.Key, null);
                continue;
            }
            
            var applicationRole = new ApplicationRole(role.Key, role.Value);
            var result = await roleManager.CreateAsync(applicationRole);

            if (result.Succeeded)
            {
                RoleSeederLogMessages.RoleSeededSuccessfully(logger, role.Key, null);
                continue;
            }
            
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            RoleSeederLogMessages.RoleSeedFailed(logger, role.Key, errors, null);
            
            throw new InvalidOperationException($"Failed to seed role '{role.Key}': {errors}");
        }
    }
}