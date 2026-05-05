namespace MedCore.Infrastructure.Localization;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

internal sealed class LocalizationDbContextFactory : IDesignTimeDbContextFactory<LocalizationDbContext>
{
    public LocalizationDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(
            Directory.GetCurrentDirectory(), "../MedCore.Api");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("PostgreSqlConnection")
                               ?? throw new InvalidOperationException("Database connection string was not found.");

        var optionsBuilder = new DbContextOptionsBuilder<LocalizationDbContext>();
        optionsBuilder
            .UseNpgsql(connectionString,
                o => o.MigrationsAssembly("MedCore.Infrastructure"))
            .UseSnakeCaseNamingConvention();

        return new LocalizationDbContext(optionsBuilder.Options);
    }
}