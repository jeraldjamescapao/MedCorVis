namespace MedCore.Modules.CodeItems.Configuration;

using MedCore.Modules.CodeItems.Application.Abstractions;
using MedCore.Modules.CodeItems.Application.Services;
using MedCore.Modules.CodeItems.Infrastructure.Persistence.Repositories;
using MedCore.Modules.CodeItems.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal static class CodeItemsModuleServiceCollectionExtensions
{
    public static IServiceCollection AddCodeItemsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServerConnection")
            ?? throw new InvalidOperationException("Database connection string is not configured.");

        services.AddDbContext<CodeItemsDbContext>(options =>
        {
            options.UseSqlServer(connectionString,
                o => o.MigrationsAssembly("MedCore.Modules.CodeItems"));
        });
        
        services.AddScoped<ICodeItemRepository, CodeItemRepository>();
        services.AddScoped<ICodeItemService, CodeItemService>();

        return services;
    }
}