namespace MedCore.Modules.Localization.Tests.Application.Services.TranslationServiceTests;

using FluentAssertions;
using MedCore.Common.Localization;
using MedCore.Common.Results;
using MedCore.Modules.Localization.Domain;
using NSubstitute;
using Xunit;

public sealed class GetAllTests : TranslationServiceTestBase
{
    [Fact]
    public async Task GetAllAsync_UnsupportedCulture_ReturnsValidation()
    {
        var result = await Sut.GetAllAsync("xx-XX");

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        result.Error!.Code.Should().Be("LOCALIZATION_UNSUPPORTED_CULTURE");
        await Repository
            .DidNotReceive()
            .GetAllAsync(Arg.Any<string?>(), Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task GetAllAsync_NullCulture_ReturnsAllTranslations()
    {
        var translations = new List<Translation>
        {
            CreateTranslation(SupportedCultures.English, "key.one", "Value One"),
            CreateTranslation(SupportedCultures.French,  "key.one", "Valeur Un")
        };

        Repository
            .GetAllAsync(null, Arg.Any<CancellationToken>())
            .Returns(translations);

        var result = await Sut.GetAllAsync(null);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task GetAllAsync_ValidCulture_ReturnsFilteredTranslations()
    {
        var translations = new List<Translation>
        {
            CreateTranslation(SupportedCultures.French, "key.one", "Valeur Un")
        };

        Repository
            .GetAllAsync(SupportedCultures.French, Arg.Any<CancellationToken>())
            .Returns(translations);

        var result = await Sut.GetAllAsync(SupportedCultures.French);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCount(1);
        result.Value![0].Culture.Should().Be(SupportedCultures.French);
    }
    
    [Fact]
    public async Task GetAllAsync_EmptyResult_ReturnsEmptyList()
    {
        Repository
            .GetAllAsync(SupportedCultures.German, Arg.Any<CancellationToken>())
            .Returns(new List<Translation>());

        var result = await Sut.GetAllAsync(SupportedCultures.German);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().BeEmpty();
    }
}