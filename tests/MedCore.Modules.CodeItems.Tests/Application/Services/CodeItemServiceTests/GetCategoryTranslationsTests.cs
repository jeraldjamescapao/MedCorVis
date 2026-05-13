namespace MedCore.Modules.CodeItems.Tests.Application.Services.CodeItemServiceTests;

using FluentAssertions;
using MedCore.Common.Results;
using MedCore.Modules.CodeItems.Domain;
using NSubstitute;
using Xunit;

public sealed class GetCategoryTranslationsTests : CodeItemServiceTestBase
{
    [Fact]
    public async Task GetCategoryTranslationsAsync_CategoryNotFound_ReturnsNotFound()
    {
        Repository
            .GetCategoryByIdAsync(69, Arg.Any<CancellationToken>())
            .Returns((Category?)null);

        var result = await Sut.GetCategoryTranslationsAsync(69);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("CODEITEMS_CATEGORY_NOT_FOUND");
    }
    
    [Fact]
    public async Task GetCategoryTranslationsAsync_NoTranslations_ReturnsEmptyList()
    {
        Repository
            .GetCategoryByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(CreateCategory());

        Repository
            .GetTranslationsByEntityAsync(
                CodeItemTranslation.EntityTypeCategory,
                1,
                Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await Sut.GetCategoryTranslationsAsync(1);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetCategoryTranslationsAsync_TranslationsExist_ReturnsCorrectCount()
    {
        Repository
            .GetCategoryByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(CreateCategory());

        Repository
            .GetTranslationsByEntityAsync(
                CodeItemTranslation.EntityTypeCategory,
                1,
                Arg.Any<CancellationToken>())
            .Returns([
                CreateTranslation(CodeItemTranslation.EntityTypeCategory, 1, "en"),
                CreateTranslation(CodeItemTranslation.EntityTypeCategory, 1, "fr")
            ]);

        var result = await Sut.GetCategoryTranslationsAsync(1);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCount(2);
    }
}