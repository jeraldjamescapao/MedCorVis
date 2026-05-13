namespace MedCore.Modules.CodeItems.Tests.Application.Services.CodeItemServiceTests;

using FluentAssertions;
using MedCore.Modules.CodeItems.Application.Contracts.Requests;
using MedCore.Modules.CodeItems.Domain;
using NSubstitute;
using Xunit;

public sealed class ReorderCategoriesTests : CodeItemServiceTestBase
{
    [Fact]
    public async Task ReorderCategoriesAsync_ValidEntries_SavesOnce()
    {
        var category = CreateCategory();

        Repository
            .GetCategoryByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(category);

        var request = new ReorderRequest([new ReorderEntry(1, 5)]);

        var result = await Sut.ReorderCategoriesAsync(request);

        result.IsSuccess.Should().BeTrue();
        await Repository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task ReorderCategoriesAsync_UnknownId_SkipsAndSaves()
    {
        Repository
            .GetCategoryByIdAsync(69, Arg.Any<CancellationToken>())
            .Returns((Category?)null);

        var request = new ReorderRequest([new ReorderEntry(69, 5)]);

        var result = await Sut.ReorderCategoriesAsync(request);

        result.IsSuccess.Should().BeTrue();
        await Repository
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}