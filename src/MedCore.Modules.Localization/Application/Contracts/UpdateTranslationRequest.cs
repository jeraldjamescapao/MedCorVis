namespace MedCore.Modules.Localization.Application.Contracts;

public sealed record UpdateTranslationRequest(
    string Value,
    string? Description);