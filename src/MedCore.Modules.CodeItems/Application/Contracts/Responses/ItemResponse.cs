namespace MedCore.Modules.CodeItems.Application.Contracts.Responses;

public sealed record ItemResponse(
    long Id,
    long CategoryId,
    string Code,
    string? Description,
    bool IsActive,
    bool IsSystemDefined,
    bool IsEditable,
    bool IsDeletable,
    bool IsDeleted,
    int SortOrder,
    DateTimeOffset CreatedAtUtc,
    string CreatedBy,
    DateTimeOffset? ModifiedAtUtc,
    string? ModifiedBy);