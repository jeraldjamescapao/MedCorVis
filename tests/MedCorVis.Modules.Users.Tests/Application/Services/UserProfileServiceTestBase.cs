namespace MedCorVis.Modules.Users.Tests.Application.Services;

using Microsoft.Extensions.Logging.Abstractions;
using MedCorVis.Common.UserProfiles;
using MedCorVis.Modules.Identity.Domain.Users;
using MedCorVis.Modules.Users.Application.Abstractions;
using MedCorVis.Modules.Users.Application.Services;
using MedCorVis.Modules.Users.Domain;
using NSubstitute;

public abstract class UserProfileServiceTestBase
{
    internal readonly IUserProfileRepository Repository;
    protected readonly IUserProfileService    Sut;

    protected UserProfileServiceTestBase()
    {
        Repository = Substitute.For<IUserProfileRepository>();

        Sut = new UserProfileService(
            Repository,
            NullLogger<UserProfileService>.Instance);
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