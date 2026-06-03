namespace MedCorVis.Modules.Users.Tests.Application.Services.UserProfileServiceTests;

using FluentAssertions;
using MedCorVis.Common.Results;
using NSubstitute;
using Xunit;

public sealed class UpdateProfileTests : UserProfileServiceTestBase
{
    private static readonly Guid UserId = Guid.NewGuid();

    [Fact]
    public async Task UpdateProfileAsync_ProfileNotFound_ReturnsNotFound()
    {
        Repository
            .GetByUserIdAsync(UserId, Arg.Any<CancellationToken>())
            .Returns((Domain.UserProfile?)null);

        var result = await Sut.UpdateProfileAsync(
            UserId, 
            "James Capao", 
            "Test Update", 
            new DateOnly(1965, 10, 10));

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("USERS_USER_NOT_FOUND");
    }

    [Fact]
    public async Task UpdateProfileAsync_ValidRequest_ReturnsUpdatedData()
    {
        var profile = CreateProfile(UserId);

        Repository
            .GetByUserIdAsync(UserId, Arg.Any<CancellationToken>())
            .Returns(profile);

        var result = await Sut.UpdateProfileAsync(
            UserId, 
            "James Capao", 
            "Test Update", 
            new DateOnly(1965, 10, 10));

        result.IsSuccess.Should().BeTrue();
        result.Value!.FirstName.Should().Be("James Capao");
        result.Value.LastName.Should().Be("Test Update");
        result.Value.FullName.Should().Be("James Capao Test Update");
        result.Value.BirthDate.Should().Be(new DateOnly(1965, 10, 10));
    }

    [Fact]
    public async Task UpdateProfileAsync_ValidRequest_SavesChanges()
    {
        var profile = CreateProfile(UserId);

        Repository
            .GetByUserIdAsync(UserId, Arg.Any<CancellationToken>())
            .Returns(profile);

        await Sut.UpdateProfileAsync(
            UserId, 
            "James Capao", 
            "Test Update", 
            new DateOnly(1965, 10, 10));

        await Repository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}