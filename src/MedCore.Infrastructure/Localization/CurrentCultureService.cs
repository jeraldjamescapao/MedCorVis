namespace MedCore.Infrastructure.Localization;

using MedCore.Common.Localization;
using MedCore.Common.Services;

internal sealed class CurrentCultureService : ICurrentCultureService
{
    public string Culture { get; private set; } = SupportedCultures.Default;

    public void SetCulture(string culture) => Culture = culture;
}