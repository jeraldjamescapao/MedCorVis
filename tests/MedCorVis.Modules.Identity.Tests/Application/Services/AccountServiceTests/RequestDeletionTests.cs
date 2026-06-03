namespace MedCorVis.Modules.Identity.Tests.Application.Services.AccountServiceTests;

using FluentAssertions;
using MedCorVis.Common.Results;
using MedCorVis.Modules.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

public sealed class RequestDeletionTests : AccountServiceTestBase
{
    [Fact]
    public async Task RequestDeletionAsync_UserNotFound_ReturnsNotFound()
    {
        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns((ApplicationUser?)null);

        var result = await Sut.RequestDeletionAsync(UserId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_USER_NOT_FOUND");
    }

    [Fact]
    public async Task RequestDeletionAsync_RequestAlreadyPending_ReturnsConflict()
    {
        var user = CreateUserWithDeletionRequest();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        var result = await Sut.RequestDeletionAsync(UserId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Conflict);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_DELETION_REQUEST_ALREADY_PENDING");
    }

    [Fact]
    public async Task RequestDeletionAsync_IdentityUpdateFails_ReturnsInternal()
    {
        var user = CreateUser();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        UserManager
            .UpdateAsync(user)
            .Returns(IdentityResult.Failed(new IdentityError
            {
                Code        = "UpdateError",
                Description = "Update failed."
            }));

        var result = await Sut.RequestDeletionAsync(UserId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Internal);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_DELETION_FAILED");
    }

    [Fact]
    public async Task RequestDeletionAsync_ValidRequest_SetsDeletionRequestedAtUtc()
    {
        var user = CreateUser();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        UserManager
            .UpdateAsync(user)
            .Returns(IdentityResult.Success);

        var result = await Sut.RequestDeletionAsync(UserId);

        result.IsSuccess.Should().BeTrue();
        user.DeletionRequestedAtUtc.Should().NotBeNull();
    }
}