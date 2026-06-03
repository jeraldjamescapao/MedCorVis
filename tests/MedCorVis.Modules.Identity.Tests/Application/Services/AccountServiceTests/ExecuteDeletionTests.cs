namespace MedCorVis.Modules.Identity.Tests.Application.Services.AccountServiceTests;

using FluentAssertions;
using MedCorVis.Common.Results;
using MedCorVis.Modules.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

public sealed class ExecuteDeletionTests : AccountServiceTestBase
{
    [Fact]
    public async Task ExecuteDeletionAsync_UserNotFound_ReturnsNotFound()
    {
        UserManager
            .FindByIdAsync(TargetId.ToString())
            .Returns((ApplicationUser?)null);

        var result = await Sut.ExecuteDeletionAsync(ActorId, TargetId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_USER_NOT_FOUND");
    }

    [Fact]
    public async Task ExecuteDeletionAsync_UserAlreadyDeleted_ReturnsUnprocessableEntity()
    {
        var user = CreateDeletedUser();

        UserManager
            .FindByIdAsync(TargetId.ToString())
            .Returns(user);

        var result = await Sut.ExecuteDeletionAsync(ActorId, TargetId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.UnprocessableEntity);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_ALREADY_DELETED");
    }

    [Fact]
    public async Task ExecuteDeletionAsync_IdentityUpdateFails_ReturnsInternal()
    {
        var user = CreateUser();

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

        var result = await Sut.ExecuteDeletionAsync(ActorId, TargetId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Internal);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_DELETION_FAILED");
    }

    [Fact]
    public async Task ExecuteDeletionAsync_ValidRequest_DeletesAndAnonymisesUser()
    {
        var user = CreateUser();

        UserManager
            .FindByIdAsync(TargetId.ToString())
            .Returns(user);

        UserManager
            .UpdateAsync(user)
            .Returns(IdentityResult.Success);

        SetupAnonymise(TargetId);

        var result = await Sut.ExecuteDeletionAsync(ActorId, TargetId);

        result.IsSuccess.Should().BeTrue();
        user.IsDeleted.Should().BeTrue();
        user.IsActive.Should().BeFalse();
        user.DeletedAtUtc.Should().NotBeNull();
        user.Email.Should().StartWith("deleted_");
        user.PhoneNumber.Should().BeNull();
        await UserProfileService
            .Received(1)
            .AnonymiseProfileAsync(TargetId, ActorId.ToString(), Arg.Any<CancellationToken>());
    }
}