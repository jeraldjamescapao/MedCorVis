namespace MedCore.Modules.Localization.Application.Contracts;

public sealed record TranslationResponse(
    long Id,
    string Culture,
    string Key,
    string Value,
    string? Description,
    bool IsActive,
    bool IsSystemDefined,
    DateTimeOffset CreatedAtUtc,
    string CreatedBy,
    DateTimeOffset? ModifiedAtUtc,
    string? ModifiedBy);