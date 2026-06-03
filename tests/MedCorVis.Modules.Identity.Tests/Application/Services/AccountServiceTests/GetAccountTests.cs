namespace MedCorVis.Modules.Identity.Tests.Application.Services.AccountServiceTests;

using FluentAssertions;
using MedCorVis.Common.Results;
using MedCorVis.Modules.Identity.Domain.Users;
using NSubstitute;
using Xunit;

public sealed class GetAccountTests : AccountServiceTestBase
{
    [Fact]
    public async Task GetAccountAsync_UserNotFound_ReturnsNotFound()
    {
        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns((ApplicationUser?)null);

        var result = await Sut.GetAccountAsync(UserId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_USER_NOT_FOUND");
    }
    
    [Fact]
    public async Task GetAccountAsync_ProfileNotFound_ReturnsNotFound()
    {
        var user = CreateUser();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        SetupProfile(UserId, null);

        var result = await Sut.GetAccountAsync(UserId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_USER_NOT_FOUND");
    }
    
    [Fact]
    public async Task GetAccountAsync_UserAndProfileExist_ReturnsCorrectShape()
    {
        var user    = CreateUser();
        var profile = CreateProfileData();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        SetupProfile(UserId, profile);

        var result = await Sut.GetAccountAsync(UserId);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Email.Should().Be("jjcapaotest@softwareengineers.ch");
        result.Value.FirstName.Should().Be("Jerald James Capao");
        result.Value.LastName.Should().Be("Test");
        result.Value.FullName.Should().Be("Jerald James Capao Test");
        result.Value.BirthDate.Should().Be(new DateOnly(1988, 6, 27));
        result.Value.IsActive.Should().BeTrue();
        result.Value.IsDeleted.Should().BeFalse();
    }
}