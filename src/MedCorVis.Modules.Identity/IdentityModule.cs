namespace MedCorVis.Modules.Identity;

using MedCorVis.Common.Modules;
using MedCorVis.Modules.Identity.Configuration;
using MedCorVis.Modules.Identity.Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public sealed class IdentityModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityModule(configuration);
        return services;
    }

    public WebApplication MapEndpoints(WebApplication app)
    {
        return app;
    }
    
    public async Task RunStartupTasksAsync(WebApplication app)
    {
        await IdentityRoleSeeder.SeedAsync(app.Services);
        await AdminUserSeeder.SeedAsync(app.Services);
    }
}