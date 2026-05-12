namespace MedCore.Modules.Localization.Tests.Application.Services.TranslationServiceTests;

using FluentAssertions;
using MedCore.Common.Localization;
using MedCore.Common.Results;
using MedCore.Modules.Localization.Application.Contracts;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

public sealed class CreateTests : TranslationServiceTestBase
{
    private static readonly CreateTranslationRequest ValidRequest = new(
        Culture:     SupportedCultures.English,
        Key:         "app.test.create",
        Value:       "Created Value",
        Description: "Test description");
    
    [Fact]
    public async Task CreateAsync_UnsupportedCulture_ReturnsValidation()
    {
        var request = ValidRequest with { Culture = "xx-XX" };

        var result = await Sut.CreateAsync(request);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        result.Error!.Code.Should().Be("LOCALIZATION_UNSUPPORTED_CULTURE");
        await Repository
            .DidNotReceive()
            .ExistsAsync(
                Arg.Any<string>(), 
                Arg.Any<string>(), 
                Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CreateAsync_DuplicateKey_ReturnsConflict()
    {
        Repository
            .ExistsAsync(
                ValidRequest.Culture, 
                ValidRequest.Key, 
                Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await Sut.CreateAsync(ValidRequest);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Conflict);
        result.Error!.Code.Should().Be("LOCALIZATION_DUPLICATE_KEY");
        await Repository
            .DidNotReceive()
            .AddAsync(
                Arg.Any<string>(), 
                Arg.Any<string>(), 
                Arg.Any<string>(),
                Arg.Any<string?>(), 
                Arg.Any<string>(), 
                Arg.Any<bool>(),
                Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task CreateAsync_SaveFails_ReturnsInternal()
    {
        var translation = CreateTranslation(
            ValidRequest.Culture, ValidRequest.Key, ValidRequest.Value);

        Repository
            .ExistsAsync(
                ValidRequest.Culture, 
                ValidRequest.Key, 
                Arg.Any<CancellationToken>())
            .Returns(false);

        Repository
            .AddAsync(
                ValidRequest.Culture, 
                ValidRequest.Key, 
                ValidRequest.Value,
                ValidRequest.Description, 
                Arg.Any<string>(), 
                false, 
                Arg.Any<CancellationToken>())
            .Returns(translation);

        Repository
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .ThrowsAsync(new InvalidOperationException("DB error"));

        var result = await Sut.CreateAsync(ValidRequest);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Internal);
        result.Error!.Code.Should().Be("LOCALIZATION_CREATE_FAILED");
        LocalizerCache.DidNotReceive().InvalidateCache();
    }
    
    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsCreatedTranslation()
    {
        var translation = CreateTranslation(
            ValidRequest.Culture, ValidRequest.Key, ValidRequest.Value);

        Repository
            .ExistsAsync(
                ValidRequest.Culture, 
                ValidRequest.Key, 
                Arg.Any<CancellationToken>())
            .Returns(false);

        Repository
            .AddAsync(
                ValidRequest.Culture, 
                ValidRequest.Key, 
                ValidRequest.Value,
                ValidRequest.Description, 
                Arg.Any<string>(), 
                Arg.Any<bool>(), 
                Arg.Any<CancellationToken>())
            .Returns(translation);

        var result = await Sut.CreateAsync(ValidRequest);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Culture.Should().Be(SupportedCultures.English);
        result.Value.Key.Should().Be("app.test.create");
        result.Value.Value.Should().Be("Created Value");
        LocalizerCache.Received(1).InvalidateCache();
        await LocalizerCache.Received(1).LoadAsync(Arg.Any<CancellationToken>());
    }
}