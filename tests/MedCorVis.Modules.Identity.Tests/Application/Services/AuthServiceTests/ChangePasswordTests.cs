namespace MedCorVis.Modules.Identity.Tests.Application.Services.AuthServiceTests;

using FluentAssertions;
using MedCorVis.Common.Results;
using MedCorVis.Modules.Identity.Application.Contracts.Authentication.Requests;
using MedCorVis.Modules.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

public sealed class ChangePasswordTests : AuthServiceTestBase
{
    private static readonly Guid UserId = Guid.NewGuid();

    private static readonly ChangePasswordRequest ValidRequest = new(
        CurrentPassword: "OldPassword123!",
        NewPassword:     "NewPassword456!");

    [Fact]
    public async Task ChangePasswordAsync_UserNotFound_ReturnsUnauthorized()
    {
        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns((ApplicationUser?)null);

        var result = await Sut.ChangePasswordAsync(UserId, ValidRequest);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Unauthorized);
        result.Error!.Code.Should().Be("IDENTITY_AUTH_INVALID_CREDENTIALS");
    }

    [Fact]
    public async Task ChangePasswordAsync_UserInactive_ReturnsUnauthorized()
    {
        var user = CreateUser(isActive: false);
        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        var result = await Sut.ChangePasswordAsync(UserId, ValidRequest);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Unauthorized);
        result.Error!.Code.Should().Be("IDENTITY_AUTH_INVALID_CREDENTIALS");
    }

    [Fact]
    public async Task ChangePasswordAsync_WrongCurrentPassword_ReturnsUnauthorized()
    {
        var user = CreateUser(emailConfirmed: true);
        UserManager
            .FindByIdAsync(user.Id.ToString())
            .Returns(user);
        UserManager
            .ChangePasswordAsync(user, Arg.Any<string>(), Arg.Any<string>())
            .Returns(IdentityResult.Failed(new IdentityError { Code = "PasswordMismatch" }));

        var result = await Sut.ChangePasswordAsync(user.Id, ValidRequest);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Unauthorized);
        result.Error!.Code.Should().Be("IDENTITY_AUTH_INVALID_CREDENTIALS");
    }

    [Fact]
    public async Task ChangePasswordAsync_IdentityFailure_ReturnsInternal()
    {
        var user = CreateUser(emailConfirmed: true);
        UserManager
            .FindByIdAsync(user.Id.ToString())
            .Returns(user);
        UserManager
            .ChangePasswordAsync(user, Arg.Any<string>(), Arg.Any<string>())
            .Returns(IdentityResult.Failed(new IdentityError { Code = "PasswordTooShort" }));

        var result = await Sut.ChangePasswordAsync(user.Id, ValidRequest);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Internal);
        result.Error!.Code.Should().Be("IDENTITY_AUTH_CHANGE_PASSWORD_FAILED");
    }

    [Fact]
    public async Task ChangePasswordAsync_ValidRequest_RevokesAllSessionsAndReturnsSuccess()
    {
        var user = CreateUser(emailConfirmed: true);
        UserManager
            .FindByIdAsync(user.Id.ToString())
            .Returns(user);
        UserManager
            .ChangePasswordAsync(user, Arg.Any<string>(), Arg.Any<string>())
            .Returns(IdentityResult.Success);

        var result = await Sut.ChangePasswordAsync(user.Id, ValidRequest);

        result.IsSuccess.Should().BeTrue();
        await RefreshTokenRepository
            .Received(1)
            .RevokeAllForUserAsync(user.Id, Arg.Any<CancellationToken>());
    }
}