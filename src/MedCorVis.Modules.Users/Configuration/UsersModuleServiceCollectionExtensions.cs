namespace MedCorVis.Modules.Users.Configuration;

using MedCorVis.Common.UserProfiles;
using MedCorVis.Modules.Users.Application.Abstractions;
using MedCorVis.Modules.Users.Application.Services;
using MedCorVis.Modules.Users.Infrastructure.Persistence;
using MedCorVis.Modules.Users.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

internal static class UsersModuleServiceCollectionExtensions
{
    public static IServiceCollection AddUsersModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServerConnection")
            ?? throw new InvalidOperationException("Database connection string is not configured.");
        
        services.AddDbContext<UsersDbContext>(options =>
            options.UseSqlServer(connectionString,
                o => o.MigrationsAssembly("MedCorVis.Modules.Users")));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IUserProfileService, UserProfileService>();

        return services;
    }
}