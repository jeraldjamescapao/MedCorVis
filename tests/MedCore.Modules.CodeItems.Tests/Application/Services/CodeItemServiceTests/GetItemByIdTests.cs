namespace MedCore.Modules.CodeItems.Tests.Application.Services.CodeItemServiceTests;

using FluentAssertions;
using MedCore.Common.Results;
using MedCore.Modules.CodeItems.Domain;
using NSubstitute;
using Xunit;

public sealed class GetItemByIdTests : CodeItemServiceTestBase
{
    [Fact]
    public async Task GetItemByIdAsync_NotFound_ReturnsNotFound()
    {
        Repository
            .GetItemByIdAsync(69, Arg.Any<CancellationToken>())
            .Returns((CodeItem?)null);

        var result = await Sut.GetItemByIdAsync(69);

        result.IsFailure.Should().BeTrue();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        result.Error!.Code.Should().Be("CODEITEMS_ITEM_NOT_FOUND");
    }
    
    [Fact]
    public async Task GetItemByIdAsync_Exists_ReturnsCorrectShape()
    {
        var item = CreateItem(code: "Consultation");

        Repository
            .GetItemByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(item);

        var result = await Sut.GetItemByIdAsync(1);

        result.IsSuccess.Should().BeTrue();
        result.Value!.Code.Should().Be("Consultation");
        result.Value.IsActive.Should().BeTrue();
        result.Value.IsEditable.Should().BeTrue();
        result.Value.IsDeletable.Should().BeTrue();
    }
}