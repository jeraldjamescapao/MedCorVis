namespace MedCore.Modules.CodeItems.Application.Contracts.Responses;

public sealed record CodeItemListResponse(
    string CategoryCode,
    IReadOnlyList<CodeItemEntry> Items);

public sealed record CodeItemEntry(
    string Code,
    string Label);