namespace MedCore.Modules.Localization.Tests.Application.Services.TranslationServiceTests;

using FluentAssertions;
using MedCore.Common.Localization;
using MedCore.Common.Results;
using MedCore.Modules.Localization.Application.Contracts;
using MedCore.Modules.Localization.Domain;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

public sealed class UpdateTests : TranslationServiceTestBase
{
    private static readonly UpdateTranslationRequest ValidRequest = new(
        Value:       "Updated Value",
        Description: "Updated description");
    
    [Fact]
    public async Task UpdateAsync_TranslationNotFound_ReturnsNotFound()
    {
        Repository
            .GetByIdAsync(69, Arg.Any<CancellationToken>())
            .Returns((Translation?)null);

        var result = await Sut.UpdateAsync(69, ValidRequest);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("LOCALIZATION_TRANSLATION_NOT_FOUND");
        await Repository
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task UpdateAsync_SaveFails_ReturnsInternal()
    {
        var translation = CreateTranslation();

        Repository
            .GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(translation);

        Repository
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("DB error"));

        var result = await Sut.UpdateAsync(1, ValidRequest);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Internal);
        result.Error!.Code.Should().Be("LOCALIZATION_UPDATE_FAILED");
        LocalizerCache.DidNotReceive().InvalidateCache();
    }
    
    [Fact]
    public async Task UpdateAsync_ValidRequest_ReturnsMutatedTranslation()
    {
        var translation = CreateTranslation(value: "Original Value");

        Repository
            .GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(translation);

        var result = await Sut.UpdateAsync(1, ValidRequest);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Value.Should().Be("Updated Value");
        result.Value.Description.Should().Be("Updated description");
        LocalizerCache.Received(1).InvalidateCache();
        await LocalizerCache.Received(1).LoadAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task UpdateAsync_SameValueAndDescription_StillReturnsSuccess()
    {
        // Translation.Update() is a no-op when nothing changes.
        // The service still calls SaveChangesAsync and reloads the cache.
        // This test confirms we do not return an error for a no-op update.
        var translation = CreateTranslation(value: "Updated Value");

        Repository
            .GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(translation);

        var result = await Sut.UpdateAsync(1, ValidRequest with { Description = null });

        result.IsSuccess.Should().BeTrue();
    }
}