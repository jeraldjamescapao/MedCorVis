namespace MedCorVis.Modules.Users.Tests.Application.Services.UserProfileServiceTests;

using FluentAssertions;
using MedCorVis.Modules.Users.Domain;
using NSubstitute;
using Xunit;

public sealed class GetProfileTests : UserProfileServiceTestBase
{
    private static readonly Guid UserId = Guid.NewGuid();
    
    [Fact]
    public async Task GetProfileAsync_ProfileNotFound_ReturnsNull()
    {
        Repository
            .GetByUserIdAsync(UserId, Arg.Any<CancellationToken>())
            .Returns((UserProfile?)null);

        var result = await Sut.GetProfileAsync(UserId);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetProfileAsync_ProfileExists_ReturnsCorrectData()
    {
        var profile = CreateProfile(UserId);

        Repository
            .GetByUserIdAsync(UserId, Arg.Any<CancellationToken>())
            .Returns(profile);

        var result = await Sut.GetProfileAsync(UserId);

        result.Should().NotBeNull();
        result!.FirstName.Should().Be(profile.FirstName);
        result.LastName.Should().Be(profile.LastName);
        result.FullName.Should().Be(profile.FullName);
        result.BirthDate.Should().Be(profile.BirthDate);
    }
}