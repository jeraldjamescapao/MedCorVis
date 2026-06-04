namespace MedCorVis.Modules.Identity.Tests.Application.Services.AuthServiceTests;

using FluentAssertions;
using MedCorVis.Common.Results;
using MedCorVis.Modules.Identity.Application.Contracts.Authentication.Requests;
using MedCorVis.Modules.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

public sealed class ResetPasswordTests : AuthServiceTestBase
{
    private static readonly ResetPasswordRequest ValidRequest = new(
        Guid.NewGuid(),
        "dmFsaWQtdG9rZW4",
        "NewPassword123");

    [Fact]
    public async Task ResetPasswordAsync_UserNotFound_ReturnsUnprocessableEntity()
    {
        UserManager
            .FindByIdAsync(ValidRequest.UserId.ToString())
            .Returns((ApplicationUser?)null);

        var result = await Sut.ResetPasswordAsync(ValidRequest);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.UnprocessableEntity);
        result.Error!.Code.Should().Be("IDENTITY_AUTH_INVALID_PASSWORD_RESET_TOKEN");
    }

    [Fact]
    public async Task ResetPasswordAsync_UserInactive_ReturnsUnprocessableEntity()
    {
        var user = CreateUser(isActive: false);
        UserManager
            .FindByIdAsync(ValidRequest.UserId.ToString())
            .Returns(user);

        var result = await Sut.ResetPasswordAsync(ValidRequest with { UserId = user.Id });

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.UnprocessableEntity);
        result.Error!.Code.Should().Be("IDENTITY_AUTH_INVALID_PASSWORD_RESET_TOKEN");
    }

    [Fact]
    public async Task ResetPasswordAsync_InvalidToken_ReturnsUnprocessableEntity()
    {
        var user = CreateUser();
        UserManager
            .FindByIdAsync(user.Id.ToString())
            .Returns(user);
        UserManager
            .ResetPasswordAsync(user, Arg.Any<string>(), Arg.Any<string>())
            .Returns(IdentityResult.Failed(new IdentityError { Code = "InvalidToken" }));

        var result = await Sut.ResetPasswordAsync(ValidRequest with { UserId = user.Id });

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.UnprocessableEntity);
        result.Error!.Code.Should().Be("IDENTITY_AUTH_INVALID_PASSWORD_RESET_TOKEN");
    }

    [Fact]
    public async Task ResetPasswordAsync_IdentityFailure_ReturnsInternal()
    {
        var user = CreateUser();
        UserManager
            .FindByIdAsync(user.Id.ToString())
            .Returns(user);
        UserManager
            .ResetPasswordAsync(user, Arg.Any<string>(), Arg.Any<string>())
            .Returns(IdentityResult.Failed(new IdentityError { Code = "PasswordTooShort" }));

        var result = await Sut.ResetPasswordAsync(ValidRequest with { UserId = user.Id });

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Internal);
        result.Error!.Code.Should().Be("IDENTITY_AUTH_PASSWORD_RESET_FAILED");
    }

    [Fact]
    public async Task ResetPasswordAsync_ValidRequest_RevokesAllSessionsAndReturnsSuccess()
    {
        var user = CreateUser();
        UserManager
            .FindByIdAsync(user.Id.ToString())
            .Returns(user);
        UserManager
            .ResetPasswordAsync(user, Arg.Any<string>(), Arg.Any<string>())
            .Returns(IdentityResult.Success);

        var result = await Sut.ResetPasswordAsync(ValidRequest with { UserId = user.Id });

        result.IsSuccess.Should().BeTrue();
        await RefreshTokenRepository
            .Received(1)
            .RevokeAllForUserAsync(user.Id, Arg.Any<CancellationToken>());
    }
}