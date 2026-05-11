namespace MedCore.Modules.Localization.Tests.Application.Services.TranslationServiceTests;

using FluentAssertions;
using MedCore.Common.Localization;
using MedCore.Common.Results;
using MedCore.Modules.Localization.Domain;
using NSubstitute;
using Xunit;

public sealed class GetByIdTests : TranslationServiceTestBase
{
    [Fact]
    public async Task GetByIdAsync_TranslationNotFound_ReturnsNotFound()
    {
        Repository
            .GetByIdAsync(69, Arg.Any<CancellationToken>())
            .Returns((Translation?)null);

        var result = await Sut.GetByIdAsync(69);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("LOCALIZATION_TRANSLATION_NOT_FOUND");
    }
    
    [Fact]
    public async Task GetByIdAsync_TranslationExists_ReturnsCorrectShape()
    {
        var translation = CreateTranslation();

        Repository
            .GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(translation);

        var result = await Sut.GetByIdAsync(1);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Culture.Should().Be(SupportedCultures.English);
        result.Value.Key.Should().Be("app.test.key");
        result.Value.Value.Should().Be("Test Value");
        result.Value.IsActive.Should().BeTrue();
    }
}