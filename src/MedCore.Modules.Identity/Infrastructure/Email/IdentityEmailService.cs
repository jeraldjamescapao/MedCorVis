namespace MedCore.Modules.Identity.Infrastructure.Email;

using Microsoft.Extensions.Options;
using MedCore.Common.Configuration;
using MedCore.Common.Localization;
using MedCore.Common.Services.Email;
using MedCore.Modules.Identity.Application.Abstractions.Email;
using MedCore.Modules.Identity.Configuration;
using MedCore.Modules.Identity.Domain.Users;

internal sealed class IdentityEmailService : IIdentityEmailService
{
    private readonly IEmailService _emailService;
    private readonly IMessageLocalizer _localizer;
    private readonly IdentityTokenSettings _identityTokenSettings;
    private readonly FrontendSettings _frontendSettings;

    public IdentityEmailService(
        IEmailService emailService,
        IMessageLocalizer localizer,
        IOptions<IdentityTokenSettings> identityTokenSettings,
        IOptions<FrontendSettings> frontendSettings)
    {
        _emailService = emailService;
        _localizer = localizer;
        _identityTokenSettings = identityTokenSettings.Value;
        _frontendSettings = frontendSettings.Value;
    }
    
    public async Task SendConfirmationEmailAsync(
        ApplicationUser user, 
        string encodedToken,
        string culture,
        CancellationToken ct = default)
    {
        var confirmationLink = 
            $"{_frontendSettings.NormalizedBaseUrl}{_identityTokenSettings.NormalizedEmailConfirmationPath}" +
            $"?userId={user.Id}&token={encodedToken}";
        
        var translations = new ConfirmationEmailTranslations(
            Subject: _localizer.Get(TranslationKeys.EmailConfirmation.Subject, culture),
            Greeting: string.Format(_localizer.Get(TranslationKeys.EmailConfirmation.Greeting, culture), 
                user.FullName),
            Instruction: _localizer.Get(TranslationKeys.EmailConfirmation.Instruction, culture),
            LinkLabel: _localizer.Get(TranslationKeys.EmailConfirmation.LinkLabel, culture),
            Expiry: string.Format(_localizer.Get(TranslationKeys.EmailConfirmation.Expiry, culture), 
                _identityTokenSettings.EmailConfirmationExpirationInHours),
            Ignore: _localizer.Get(TranslationKeys.EmailConfirmation.Ignore, culture),
            Closing: _localizer.Get(TranslationKeys.EmailConfirmation.Closing, culture),
            AppName: _localizer.Get(TranslationKeys.AppGeneral.Name, culture));
        
        var message = new EmailMessage(
            To: user.Email!,
            Subject: translations.Subject,
            HtmlBody: BuildHtmlBody(confirmationLink, translations),
            PlainTextBody: BuildPlainTextBody(confirmationLink, translations));

        await _emailService.SendAsync(message, ct);
    }

    private static string BuildHtmlBody(string confirmationLink, ConfirmationEmailTranslations translations)
    {
        return $"""
                <p>{translations.Greeting}</p>
                <p>{translations.Instruction}</p>
                <p><a href="{confirmationLink}">{translations.LinkLabel}</a></p>
                <p>{translations.Expiry}</p>
                <p>{translations.Ignore}</p>
                <p>{translations.Closing}</p>
                <p>{translations.AppName}</p>
                """;
    }

    private static string BuildPlainTextBody(string confirmationLink, ConfirmationEmailTranslations translations)
    {
        return $"""
                {translations.Greeting}

                {translations.Instruction}

                {confirmationLink}

                {translations.Expiry}

                {translations.Ignore}
                
                {translations.Closing}
                
                {translations.AppName}
                """;
    }
    
    private sealed record ConfirmationEmailTranslations(
        string Subject,
        string Greeting,
        string Instruction,
        string LinkLabel,
        string Expiry,
        string Ignore,
        string Closing,
        string AppName);
}