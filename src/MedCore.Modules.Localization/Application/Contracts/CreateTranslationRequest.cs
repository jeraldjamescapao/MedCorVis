namespace MedCore.Modules.Localization.Application.Contracts;

public sealed record CreateTranslationRequest(
    string Culture,
    string Key,
    string Value,
    string? Description);