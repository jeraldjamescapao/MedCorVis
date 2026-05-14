namespace MedCorVis.Modules.Users.Configuration;

using Microsoft.Extensions.DependencyInjection;
using MedCorVis.Modules.Users.Application.Abstractions;
using MedCorVis.Modules.Users.Application.Services;

internal static class UsersModuleServiceCollectionExtensions
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        
        return services;
    }
}