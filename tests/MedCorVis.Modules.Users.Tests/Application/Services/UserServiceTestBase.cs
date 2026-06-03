namespace MedCorVis.Modules.Users.Tests.Application.Services;

using Microsoft.Extensions.Logging.Abstractions;
using MedCorVis.Modules.Users.Application.Abstractions;
using MedCorVis.Modules.Users.Application.Services;
using MedCorVis.Modules.Users.Domain;
using MedCorVis.Modules.Identity.Domain.Users;
using NSubstitute;

public abstract class UserServiceTestBase
{
    internal readonly IUserProfileRepository Repository;
    protected readonly IUserService           Sut;

    protected UserServiceTestBase()
    {
        Repository = Substitute.For<IUserProfileRepository>();

        Sut = new UserService(
            Repository,
            NullLogger<UserService>.Instance);
    }

    protected void SetupProfile(Guid userId, UserProfile? profile)
    {
        Repository
            .GetByUserIdAsync(userId, Arg.Any<CancellationToken>())
            .Returns(profile);
    }

    protected static UserProfile CreateProfile(Guid userId)
    {
        return UserProfile.Create(
            userId,
            firstName: "Jerald James Capao",
            lastName:  "Test",
            birthDate: new DateOnly(1988, 6, 27),
            createdBy: ApplicationUser.SelfRegisteredActor);
    }
}