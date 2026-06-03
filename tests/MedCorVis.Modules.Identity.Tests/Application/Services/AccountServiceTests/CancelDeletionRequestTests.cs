namespace MedCorVis.Modules.Identity.Tests.Application.Services.AccountServiceTests;

using FluentAssertions;
using MedCorVis.Common.Results;
using MedCorVis.Modules.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

public sealed class CancelDeletionRequestTests : AccountServiceTestBase
{
    [Fact]
    public async Task CancelDeletionRequestAsync_UserNotFound_ReturnsNotFound()
    {
        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns((ApplicationUser?)null);

        var result = await Sut.CancelDeletionRequestAsync(UserId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_USER_NOT_FOUND");
    }

    [Fact]
    public async Task CancelDeletionRequestAsync_NoPendingRequest_ReturnsUnprocessableEntity()
    {
        var user = CreateUser();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        var result = await Sut.CancelDeletionRequestAsync(UserId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.UnprocessableEntity);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_NO_DELETION_REQUEST_PENDING");
    }

    [Fact]
    public async Task CancelDeletionRequestAsync_IdentityUpdateFails_ReturnsInternal()
    {
        var user = CreateUserWithDeletionRequest();

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

        var result = await Sut.CancelDeletionRequestAsync(UserId);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Internal);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_DELETION_FAILED");
    }

    [Fact]
    public async Task CancelDeletionRequestAsync_PendingRequest_ClearsDeletionRequestedAtUtc()
    {
        var user = CreateUserWithDeletionRequest();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        UserManager
            .UpdateAsync(user)
            .Returns(IdentityResult.Success);

        var result = await Sut.CancelDeletionRequestAsync(UserId);

        result.IsSuccess.Should().BeTrue();
        user.DeletionRequestedAtUtc.Should().BeNull();
    }
}