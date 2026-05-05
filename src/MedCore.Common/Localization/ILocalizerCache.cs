namespace MedCore.Common.Localization;

public interface ILocalizerCache
{
    Task LoadAsync(CancellationToken ct = default);
    void InvalidateCache();
}