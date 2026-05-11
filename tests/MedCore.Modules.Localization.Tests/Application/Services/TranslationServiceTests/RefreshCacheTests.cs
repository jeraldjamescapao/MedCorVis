namespace MedCore.Modules.Localization.Tests.Application.Services.TranslationServiceTests;

using NSubstitute;
using Xunit;

public sealed class RefreshCacheTests : TranslationServiceTestBase
{
    [Fact]
    public async Task RefreshCacheAsync_Always_InvalidatesAndReloadsCache()
    {
        await Sut.RefreshCacheAsync();

        LocalizerCache.Received(1).InvalidateCache();
        await LocalizerCache.Received(1).LoadAsync(Arg.Any<CancellationToken>());
    }
}