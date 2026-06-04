namespace MedCorVis.Modules.Identity.Tests.Application.Services.AuthServiceTests;

using FluentAssertions;
using MedCorVis.Common.Exceptions;
using MedCorVis.Common.Results;
using MedCorVis.Modules.Identity.Application.Contracts.Authentication.Requests;
using MedCorVis.Modules.Identity.Domain.Users;
using NSubstitute;
using Xunit;

public sealed class ForgotPasswordTests : AuthServiceTestBase
{
    private static readonly ForgotPasswordRequest ValidRequest =
        new("jjcapaotest@softwareengineers.ch");

    [Fact]
    public async Task ForgotPasswordAsync_UserNotFound_SilentlySucceeds()
    {
        UserManager
            .FindByEmailAsync(ValidRequest.Email)
            .Returns((ApplicationUser?)null);

        var result = await Sut.ForgotPasswordAsync(ValidRequest);

        result.IsSuccess.Should().BeTrue();
        await IdentityEmailService
            .DidNotReceive()
            .SendPasswordResetEmailAsync(
                Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ForgotPasswordAsync_UserInactive_SilentlySucceeds()
    {
        var user = CreateUser(isActive: false);
        UserManager
            .FindByEmailAsync(ValidRequest.Email)
            .Returns(user);

        var result = await Sut.ForgotPasswordAsync(ValidRequest);

        result.IsSuccess.Should().BeTrue();
        await IdentityEmailService
            .DidNotReceive()
            .SendPasswordResetEmailAsync(
                Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ForgotPasswordAsync_EmailNotConfirmed_SilentlySucceeds()
    {
        var user = CreateUser(emailConfirmed: false);
        UserManager
            .FindByEmailAsync(ValidRequest.Email)
            .Returns(user);

        var result = await Sut.ForgotPasswordAsync(ValidRequest);

        result.IsSuccess.Should().BeTrue();
        await IdentityEmailService
            .DidNotReceive()
            .SendPasswordResetEmailAsync(
                Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ForgotPasswordAsync_EmailDeliveryFails_ReturnsServiceUnavailable()
    {
        var user = CreateUser(emailConfirmed: true);
        UserManager
            .FindByEmailAsync(ValidRequest.Email)
            .Returns(user);
        UserManager
            .GeneratePasswordResetTokenAsync(user)
            .Returns("raw-token");
        
        IdentityEmailService
            .SendPasswordResetEmailAsync(
                Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(new EmailDeliveryException("SMTP failed.")));

        var result = await Sut.ForgotPasswordAsync(ValidRequest);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.ServiceUnavailable);
        result.Error!.Code.Should().Be("IDENTITY_AUTH_EMAIL_DELIVERY_FAILED");
    }

    [Fact]
    public async Task ForgotPasswordAsync_ValidRequest_ReturnsSuccess()
    {
        var user = CreateUser(emailConfirmed: true);
        UserManager
            .FindByEmailAsync(ValidRequest.Email)
            .Returns(user);
        UserManager
            .GeneratePasswordResetTokenAsync(user)
            .Returns("raw-token");
        
        IdentityEmailService
            .SendPasswordResetEmailAsync(
                Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        var result = await Sut.ForgotPasswordAsync(ValidRequest);

        result.IsSuccess.Should().BeTrue();
        await IdentityEmailService
            .Received(1)
            .SendPasswordResetEmailAsync(
                Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}