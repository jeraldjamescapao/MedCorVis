namespace MedCore.Modules.CodeItems.Tests.Application.Services.CodeItemServiceTests;

using FluentAssertions;
using MedCore.Common.Results;
using MedCore.Modules.CodeItems.Domain;
using NSubstitute;
using Xunit;

public sealed class GetItemTranslationsTests : CodeItemServiceTestBase
{
    [Fact]
    public async Task GetItemTranslationsAsync_ItemNotFound_ReturnsNotFound()
    {
        Repository
            .GetItemByIdAsync(69, Arg.Any<CancellationToken>())
            .Returns((CodeItem?)null);

        var result = await Sut.GetItemTranslationsAsync(1, 69);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("CODEITEMS_ITEM_NOT_FOUND");
    }
    
    [Fact]
    public async Task GetItemTranslationsAsync_NoTranslations_ReturnsEmptyList()
    {
        Repository
            .GetItemByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(CreateItem());

        Repository
            .GetTranslationsByEntityAsync(
                CodeItemTranslation.EntityTypeItem,
                1,
                Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await Sut.GetItemTranslationsAsync(1, 1);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
    
    [Fact]
    public async Task GetItemTranslationsAsync_TranslationsExist_ReturnsCorrectCount()
    {
        Repository
            .GetItemByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(CreateItem());

        Repository
            .GetTranslationsByEntityAsync(
                CodeItemTranslation.EntityTypeItem,
                1,
                Arg.Any<CancellationToken>())
            .Returns([
                CreateTranslation(CodeItemTranslation.EntityTypeItem, 1, "en"),
                CreateTranslation(CodeItemTranslation.EntityTypeItem, 1, "fr"),
                CreateTranslation(CodeItemTranslation.EntityTypeItem, 1, "de")
            ]);

        var result = await Sut.GetItemTranslationsAsync(1, 1);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Should().HaveCount(3);
    }
    
    [Fact]
    public async Task GetItemTranslationsAsync_ItemBelongsToDifferentCategory_ReturnsNotFound()
    {
        var item = CreateItem(categoryId: 99);

        Repository
            .GetItemByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(item);

        var result = await Sut.GetItemTranslationsAsync(categoryId: 1, itemId: 1);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("CODEITEMS_ITEM_NOT_FOUND");
    }
}