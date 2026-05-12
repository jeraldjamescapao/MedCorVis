namespace MedCore.Modules.CodeItems.Application.Contracts.Responses;

public sealed record TranslationResponse(
    long Id,
    string EntityType,
    long EntityId,
    string Culture,
    string Label,
    string? Description,
    bool IsSystemDefined,
    bool IsActive,
    DateTimeOffset CreatedAtUtc,
    string CreatedBy,
    DateTimeOffset? ModifiedAtUtc,
    string? ModifiedBy);