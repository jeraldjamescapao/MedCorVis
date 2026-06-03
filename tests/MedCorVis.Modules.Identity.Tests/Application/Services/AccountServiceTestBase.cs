namespace MedCorVis.Modules.Identity.Tests.Application.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using MedCorVis.Common.Account;
using MedCorVis.Common.Caching;
using MedCorVis.Common.UserProfiles;
using MedCorVis.Modules.Identity.Application.Services;
using MedCorVis.Modules.Identity.Domain.Users;
using MedCorVis.Modules.Identity.Tests.Helpers;
using NSubstitute;

public abstract class AccountServiceTestBase
{
    protected readonly UserManager<ApplicationUser> UserManager;
    protected readonly IUserProfileService          UserProfileService;
    protected readonly IUserCultureCache            UserCultureCache;
    protected readonly IAccountService              Sut;

    protected static readonly Guid UserId   = Guid.NewGuid();
    protected static readonly Guid ActorId  = Guid.NewGuid();
    protected static readonly Guid TargetId = Guid.NewGuid();
    
    protected AccountServiceTestBase()
    {
        UserManager        = MockUserManager.Create();
        UserProfileService = Substitute.For<IUserProfileService>();
        UserCultureCache   = Substitute.For<IUserCultureCache>();

        Sut = new AccountService(
            UserManager,
            UserProfileService,
            UserCultureCache,
            NullLogger<AccountService>.Instance);
    }
    
    protected static ApplicationUser CreateUser(
        string email = "jjcapaotest@softwareengineers.ch")
    {
        return ApplicationUser.Create(email, createdBy: ApplicationUser.SelfRegisteredActor);
    }

    protected static ApplicationUser CreateDeletedUser()
    {
        var user = CreateUser();
        user.Delete(ActorId.ToString());
        return user;
    }

    protected static ApplicationUser CreateUserWithDeletionRequest()
    {
        var user = CreateUser();
        user.RequestDeletion();
        return user;
    }

    protected static UserProfileData CreateProfileData()
    {
        return new UserProfileData("Jerald James Capao", 
            "Test", 
            "Jerald James Capao Test", 
            new DateOnly(1988, 6, 27));
    }
    
    protected void SetupProfile(Guid userId, UserProfileData? data)
    {
        UserProfileService
            .GetProfileAsync(userId, Arg.Any<CancellationToken>())
            .Returns(data);
    }

    protected void SetupAnonymise(Guid userId)
    {
        UserProfileService
            .AnonymiseProfileAsync(userId, Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
    }
}