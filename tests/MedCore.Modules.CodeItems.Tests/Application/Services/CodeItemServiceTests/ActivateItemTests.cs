namespace MedCore.Modules.CodeItems.Tests.Application.Services.CodeItemServiceTests;

using FluentAssertions;
using MedCore.Common.Results;
using MedCore.Modules.CodeItems.Domain;
using NSubstitute;
using Xunit;

public sealed class ActivateItemTests : CodeItemServiceTestBase
{
    [Fact]
    public async Task ActivateItemAsync_NotFound_ReturnsNotFound()
    {
        Repository
            .GetItemByIdAsync(69, Arg.Any<CancellationToken>())
            .Returns((CodeItem?)null);

        var result = await Sut.ActivateItemAsync(1, 69);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("CODEITEMS_ITEM_NOT_FOUND");
    }
    
    [Fact]
    public async Task ActivateItemAsync_ValidRequest_ActivatesAndSaves()
    {
        var item = CreateItem(isActive: false);

        Repository
            .GetItemByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(item);

        var result = await Sut.ActivateItemAsync(1, 1);

        result.IsSuccess.Should().BeTrue();
        item.IsActive.Should().BeTrue();
        await Repository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task ActivateItemAsync_ItemBelongsToDifferentCategory_ReturnsNotFound()
    {
        var item = CreateItem(categoryId: 99);

        Repository
            .GetItemByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(item);

        var result = await Sut.ActivateItemAsync(categoryId: 1, id: 1);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("CODEITEMS_ITEM_NOT_FOUND");
        await Repository
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}