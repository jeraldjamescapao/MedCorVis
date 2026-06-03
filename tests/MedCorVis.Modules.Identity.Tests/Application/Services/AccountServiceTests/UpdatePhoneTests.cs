namespace MedCorVis.Modules.Identity.Tests.Application.Services.AccountServiceTests;

using FluentAssertions;
using MedCorVis.Common.Results;
using MedCorVis.Modules.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

public sealed class UpdatePhoneTests : AccountServiceTestBase
{
    [Fact]
    public async Task UpdatePhoneAsync_UserNotFound_ReturnsNotFound()
    {
        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns((ApplicationUser?)null);

        var result = await Sut.UpdatePhoneAsync(UserId, "+41791234567");

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_USER_NOT_FOUND");
    }

    [Fact]
    public async Task UpdatePhoneAsync_IdentityUpdateFails_ReturnsInternal()
    {
        var user = CreateUser();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        UserManager
            .SetPhoneNumberAsync(user, Arg.Any<string?>())
            .Returns(IdentityResult.Failed(new IdentityError
            {
                Code        = "PhoneError",
                Description = "Phone update failed."
            }));

        var result = await Sut.UpdatePhoneAsync(UserId, "+41791234567");

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Internal);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_PHONE_UPDATE_FAILED");
    }

    [Fact]
    public async Task UpdatePhoneAsync_ValidPhone_Succeeds()
    {
        var user = CreateUser();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        UserManager
            .SetPhoneNumberAsync(user, "+41791234567")
            .Returns(IdentityResult.Success);

        var result = await Sut.UpdatePhoneAsync(UserId, "+41791234567");

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task UpdatePhoneAsync_NullPhone_Succeeds()
    {
        var user = CreateUser();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        UserManager
            .SetPhoneNumberAsync(user, null)
            .Returns(IdentityResult.Success);

        var result = await Sut.UpdatePhoneAsync(UserId, null);

        result.IsSuccess.Should().BeTrue();
    }
}