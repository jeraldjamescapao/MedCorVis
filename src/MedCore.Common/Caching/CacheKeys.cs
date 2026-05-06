namespace MedCore.Common.Caching;

public static class CacheKeys
{
    public static string UserCulture(Guid userId) => $"culture:{userId}";

    public const string Translations = "localizer:translations";
}