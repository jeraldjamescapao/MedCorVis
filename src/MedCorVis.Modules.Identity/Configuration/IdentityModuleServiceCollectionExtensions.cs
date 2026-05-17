namespace MedCorVis.Modules.Identity.Configuration;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MedCorVis.Common.Services;
using MedCorVis.Modules.Identity.Application.Abstractions.Authentication;
using MedCorVis.Modules.Identity.Application.Abstractions.Email;
using MedCorVis.Modules.Identity.Application.Abstractions.Persistence;
using MedCorVis.Modules.Identity.Application.Services;
using MedCorVis.Modules.Identity.Domain.Roles;
using MedCorVis.Modules.Identity.Domain.Tokens;
using MedCorVis.Modules.Identity.Domain.Users;
using MedCorVis.Modules.Identity.Infrastructure.Persistence;
using MedCorVis.Modules.Identity.Infrastructure.Persistence.Repositories;
using MedCorVis.Modules.Identity.Infrastructure.Services;
using MedCorVis.Modules.Identity.Infrastructure.Services.Authentication;
using MedCorVis.Modules.Identity.Infrastructure.Services.BackgroundServices;
using MedCorVis.Modules.Identity.Infrastructure.Services.Email;
using System.Text;
    
internal static class IdentityModuleServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServerConnection") 
            ?? throw new InvalidOperationException("Database connection string is not configured.");
        
        services.AddIdentityPersistence(connectionString);
        services.AddIdentityServices(configuration);
        services.AddIdentityJwt(configuration);
        
        return services;
    }

    private static IServiceCollection AddIdentityPersistence(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<IdentityDbContext>(options =>
        {
            options.UseSqlServer(connectionString,
                o => o.MigrationsAssembly("MedCorVis.Modules.Identity"));
        });
        
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
        
        return services;
    }

    private static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDataProtection();
        
        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = ApplicationUser.PasswordMinLength;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<ICultureResolver, IdentityCultureResolver>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IIdentityEmailService, IdentityEmailService>();

        services.AddOptions<IdentityTokenSettings>()
            .BindConfiguration(IdentityTokenSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            var tokenSettings = configuration.GetSection(IdentityTokenSettings.SectionName)
                .Get<IdentityTokenSettings>() 
                    ?? throw new InvalidOperationException("IdentityTokens settings are not configured.");
            
            options.TokenLifespan = TimeSpan.FromHours(tokenSettings.EmailConfirmationExpirationInHours);
        });
        
        services.AddOptions<RefreshTokenCleanupSettings>()
            .BindConfiguration(RefreshTokenCleanupSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddHostedService<RefreshTokenCleanupService>();
        
        return services;       
    }

    private static IServiceCollection AddIdentityJwt(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<JwtSettings>()
            .BindConfiguration(JwtSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings are not configured.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        
        return services;      
    }
}