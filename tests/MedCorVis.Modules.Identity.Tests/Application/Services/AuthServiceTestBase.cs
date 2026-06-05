namespace MedCorVis.Modules.Identity.Tests.Application.Services;

using MedCorVis.Common.Authorization;
using MedCorVis.Common.Caching;
using MedCorVis.Common.Localization;
using MedCorVis.Common.Services;
using MedCorVis.Common.UserProfiles;
using MedCorVis.Modules.Identity.Application.Abstractions.Authentication;
using MedCorVis.Modules.Identity.Application.Abstractions.Email;
using MedCorVis.Modules.Identity.Application.Abstractions.Persistence;
using MedCorVis.Modules.Identity.Application.Services;
using MedCorVis.Modules.Identity.Configuration;
using MedCorVis.Modules.Identity.Domain.Tokens;
using MedCorVis.Modules.Identity.Domain.Users;
using MedCorVis.Modules.Identity.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;

public abstract class AuthServiceTestBase
{
    protected readonly ISessionContext SessionContext;
    protected readonly UserManager<ApplicationUser> UserManager;
    internal readonly IJwtTokenService JwtTokenService;
    internal readonly IRefreshTokenRepository RefreshTokenRepository;
    internal readonly IIdentityEmailService IdentityEmailService;
    internal readonly IIdentityUnitOfWork UnitOfWork;
    protected readonly IDbContextTransaction Transaction;
    protected readonly ICurrentCultureService CurrentCultureService;
    protected readonly IUserCultureCache UserCultureCache;
    protected readonly IUserProfileService UserProfileService;
    protected readonly IAuthService Sut;
    
    protected static readonly JwtSettings DefaultJwtSettings = new()
    {
        SecretKey  = "TesterJamesCapaoLongSecretKeyGoesLikeThis",
        Issuer     = "MedCorVis",
        Audience   = "MedCorVis",
        AccessTokenExpirationInMinutes = 15,
        RefreshTokenExpirationInDays   = 7
    };
    
    protected AuthServiceTestBase()
    {
        SessionContext = Substitute.For<ISessionContext>();
        SessionContext.IpAddress.Returns("127.0.0.1");
        SessionContext.UserAgent.Returns("TestAgent/1.0");
        
        UserManager            = MockUserManager.Create();
        JwtTokenService        = Substitute.For<IJwtTokenService>();
        RefreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        IdentityEmailService   = Substitute.For<IIdentityEmailService>();
        UnitOfWork             = Substitute.For<IIdentityUnitOfWork>();
        Transaction            = Substitute.For<IDbContextTransaction>();
        
        CurrentCultureService = Substitute.For<ICurrentCultureService>();
        CurrentCultureService.Culture.Returns(SupportedCultures.Default);
        UserCultureCache = Substitute.For<IUserCultureCache>();
        UserProfileService = Substitute.For<IUserProfileService>();
        
        UnitOfWork
            .BeginTransactionAsync(Arg.Any<CancellationToken>())
            .Returns(Transaction);
        
        JwtTokenService.GenerateAccessToken(Arg.Any<ApplicationUser>(), Arg.Any<IList<string>>())
            .Returns("access-token");

        JwtTokenService.GenerateRefreshToken()
            .Returns("raw-refresh-token");
        
        Sut = new AuthService(
            SessionContext,
            UserManager,
            CurrentCultureService,
            UserCultureCache,
            UserProfileService,
            JwtTokenService,
            RefreshTokenRepository,
            IdentityEmailService,
            UnitOfWork,
            Options.Create(DefaultJwtSettings),
            NullLogger<AuthService>.Instance);
    }
    
    protected static ApplicationUser CreateUser(
        string email        = "jjcapaotest@softwareengineers.ch",
        bool   isActive     = true,
        bool   emailConfirmed = false)
    {
        var user = ApplicationUser.Create(
            email,
            createdBy: ApplicationUser.SelfRegisteredActor);

        typeof(ApplicationUser)
            .GetProperty(nameof(ApplicationUser.EmailConfirmed))!
            .SetValue(user, emailConfirmed);

        if (!isActive)
            user.Deactivate(SystemActors.System);

        return user;
    }
    
    protected void SetupUserProfileFullName(Guid userId, string? fullName)
    {
        UserProfileService
            .GetFullNameAsync(userId, Arg.Any<CancellationToken>())
            .Returns(fullName);
    }
}