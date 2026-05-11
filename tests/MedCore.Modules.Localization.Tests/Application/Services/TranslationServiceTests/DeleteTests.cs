namespace MedCore.Modules.Localization.Tests.Application.Services.TranslationServiceTests;

using FluentAssertions;
using MedCore.Common.Results;
using MedCore.Modules.Localization.Domain;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

public sealed class DeleteTests : TranslationServiceTestBase
{   
    [Fact]
    public async Task DeleteAsync_TranslationNotFound_ReturnsNotFound()
    {
        Repository
            .GetByIdAsync(69, Arg.Any<CancellationToken>())
            .Returns((Translation?)null);

        var result = await Sut.DeleteAsync(69);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("LOCALIZATION_TRANSLATION_NOT_FOUND");
        await Repository
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task DeleteAsync_SaveFails_ReturnsInternal()
    {
        var translation = CreateTranslation();

        Repository
            .GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(translation);

        Repository
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("DB error"));

        var result = await Sut.DeleteAsync(1);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Internal);
        result.Error!.Code.Should().Be("LOCALIZATION_DELETE_FAILED");
        LocalizerCache.DidNotReceive().InvalidateCache();
    }
    
    [Fact]
    public async Task DeleteAsync_ActiveTranslation_DeactivatesAndSucceeds()
    {
        var translation = CreateTranslation(isActive: true);

        Repository
            .GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(translation);

        var result = await Sut.DeleteAsync(1);

        result.IsSuccess.Should().BeTrue();
        translation.IsActive.Should().BeFalse();
        LocalizerCache.Received(1).InvalidateCache();
        await LocalizerCache.Received(1).LoadAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task DeleteAsync_AlreadyInactiveTranslation_StillSucceeds()
    {
        // Translation.Deactivate() is a no-op when already inactive.
        // The service still saves and reloads the cache.
        var translation = CreateTranslation(isActive: false);

        Repository
            .GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(translation);

        var result = await Sut.DeleteAsync(1);

        result.IsSuccess.Should().BeTrue();
        translation.IsActive.Should().BeFalse();
    }
}