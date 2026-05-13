namespace MedCore.Modules.CodeItems.Tests.Application.Services.CodeItemServiceTests;

using FluentAssertions;
using MedCore.Common.Results;
using MedCore.Modules.CodeItems.Application.Contracts.Requests;
using MedCore.Modules.CodeItems.Domain;
using NSubstitute;
using Xunit;

public sealed class ReorderItemsTests : CodeItemServiceTestBase
{
    [Fact]
    public async Task ReorderItemsAsync_CategoryNotFound_ReturnsNotFound()
    {
        Repository
            .GetCategoryByIdAsync(69, Arg.Any<CancellationToken>())
            .Returns((Category?)null);

        var request = new ReorderRequest([new ReorderEntry(1, 5)]);

        var result = await Sut.ReorderItemsAsync(69, request);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("CODEITEMS_CATEGORY_NOT_FOUND");
        await Repository
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task ReorderItemsAsync_ValidEntries_SavesOnce()
    {
        Repository
            .GetCategoryByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(CreateCategory());

        var item = CreateItem();

        Repository
            .GetItemByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(item);

        var request = new ReorderRequest([new ReorderEntry(1, 5)]);

        var result = await Sut.ReorderItemsAsync(1, request);

        result.IsSuccess.Should().BeTrue();
        await Repository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}