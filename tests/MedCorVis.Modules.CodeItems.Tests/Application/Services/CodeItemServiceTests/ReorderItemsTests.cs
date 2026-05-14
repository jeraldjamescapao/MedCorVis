namespace MedCorVis.Modules.CodeItems.Tests.Application.Services.CodeItemServiceTests;

using FluentAssertions;
using MedCorVis.Common.Results;
using MedCorVis.Modules.CodeItems.Application.Contracts.Requests;
using MedCorVis.Modules.CodeItems.Domain;
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
            .GetTrackedItemsByCategoryIdAndIdsAsync(
                1,
                Arg.Is<IReadOnlyCollection<long>>(ids => ids.Contains(item.Id)),
                Arg.Any<CancellationToken>())
            .Returns([item]);

        var request = new ReorderRequest([new ReorderEntry(item.Id, 5)]);

        var result = await Sut.ReorderItemsAsync(1, request);

        result.IsSuccess.Should().BeTrue();
        await Repository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task ReorderItemsAsync_UnknownItemId_ReturnsNotFound()
    {
        Repository
            .GetCategoryByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(CreateCategory());

        Repository
            .GetTrackedItemsByCategoryIdAndIdsAsync(
                1,
                Arg.Is<IReadOnlyCollection<long>>(ids => ids.Contains(69)),
                Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CodeItem>>([]));

        var request = new ReorderRequest([new ReorderEntry(69, 5)]);

        var result = await Sut.ReorderItemsAsync(1, request);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);

        await Repository
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}