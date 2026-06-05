namespace MedCorVis.Modules.Identity.Tests.Application.Services.AccountServiceTests;

using FluentAssertions;
using MedCorVis.Common.Results;
using MedCorVis.Modules.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

public sealed class ActivateUserTests : AccountServiceTestBase
{
    [Fact]
    public async Task ActivateUserAsync_UserNotFound_ReturnsNotFound()
    {
        UserManager
            .FindByIdAsync(TargetId.ToString())
            .Returns((ApplicationUser?)null);

        var result = await Sut.ActivateUserAsync(ActorId, TargetId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_USER_NOT_FOUND");
    }

    [Fact]
    public async Task ActivateUserAsync_UserAlreadyActive_ReturnsConflict()
    {
        var user = CreateUser(); // IsActive = true by default

        UserManager
            .FindByIdAsync(TargetId.ToString())
            .Returns(user);

        var result = await Sut.ActivateUserAsync(ActorId, TargetId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Conflict);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_ALREADY_ACTIVE");
    }

    [Fact]
    public async Task ActivateUserAsync_IdentityUpdateFails_ReturnsInternal()
    {
        var user = CreateUser();
        user.Deactivate(ActorId.ToString());

        UserManager
            .FindByIdAsync(TargetId.ToString())
            .Returns(user);

        UserManager
            .UpdateAsync(user)
            .Returns(IdentityResult.Failed(new IdentityError
            {
                Code        = "UpdateError",
                Description = "Update failed."
            }));

        var result = await Sut.ActivateUserAsync(ActorId, TargetId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Internal);
    }

    [Fact]
    public async Task ActivateUserAsync_InactiveUser_ActivatesAndSucceeds()
    {
        var user = CreateUser();
        user.Deactivate(ActorId.ToString());

        UserManager
            .FindByIdAsync(TargetId.ToString())
            .Returns(user);

        UserManager
            .UpdateAsync(user)
            .Returns(IdentityResult.Success);

        var result = await Sut.ActivateUserAsync(ActorId, TargetId);

        result.IsSuccess.Should().BeTrue();
        user.IsActive.Should().BeTrue();
    }
}