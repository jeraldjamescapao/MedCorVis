namespace MedCorVis.Modules.Identity.Tests.Application.Services.AccountServiceTests;

using FluentAssertions;
using MedCorVis.Common.Localization;
using MedCorVis.Common.Results;
using MedCorVis.Modules.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Xunit;

public sealed class UpdateCultureTests : AccountServiceTestBase
{
    [Fact]
    public async Task UpdateCultureAsync_UnsupportedCulture_ReturnsValidation()
    {
        var result = await Sut.UpdateCultureAsync(UserId, "xxxyyyzzz");

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_UNSUPPORTED_CULTURE");
        await UserManager
            .DidNotReceive()
            .FindByIdAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task UpdateCultureAsync_UserNotFound_ReturnsNotFound()
    {
        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns((ApplicationUser?)null);

        var result = await Sut.UpdateCultureAsync(UserId, SupportedCultures.French);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_USER_NOT_FOUND");
    }

    [Fact]
    public async Task UpdateCultureAsync_IdentityUpdateFails_ReturnsInternal()
    {
        var user = CreateUser();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        UserManager
            .UpdateAsync(user)
            .Returns(IdentityResult.Failed(new IdentityError
            {
                Code        = "CultureError",
                Description = "Culture update failed."
            }));

        var result = await Sut.UpdateCultureAsync(UserId, SupportedCultures.French);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Internal);
        result.Error!.Code.Should().Be("IDENTITY_ACCOUNT_CULTURE_UPDATE_FAILED");
        UserCultureCache
            .DidNotReceive()
            .SetCultureForUser(UserId, Arg.Any<string>());
    }

    [Fact]
    public async Task UpdateCultureAsync_ValidCulture_UpdatesAndSucceeds()
    {
        var user = CreateUser();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        UserManager
            .UpdateAsync(user)
            .Returns(IdentityResult.Success);

        var result = await Sut.UpdateCultureAsync(UserId, SupportedCultures.French);

        result.IsSuccess.Should().BeTrue();
        user.PreferredCulture.Should().Be(SupportedCultures.French);
        UserCultureCache
            .Received(1)
            .SetCultureForUser(UserId, SupportedCultures.French);
    }

    [Fact]
    public async Task UpdateCultureAsync_RegionalCulture_AcceptsAndSucceeds()
    {
        var user = CreateUser();

        UserManager
            .FindByIdAsync(UserId.ToString())
            .Returns(user);

        UserManager
            .UpdateAsync(user)
            .Returns(IdentityResult.Success);

        var result = await Sut.UpdateCultureAsync(UserId, SupportedCultures.FrenchSwitzerland);

        result.IsSuccess.Should().BeTrue();
        user.PreferredCulture.Should().Be(SupportedCultures.FrenchSwitzerland);
        UserCultureCache
            .Received(1)
            .SetCultureForUser(UserId, SupportedCultures.FrenchSwitzerland);
    }
}