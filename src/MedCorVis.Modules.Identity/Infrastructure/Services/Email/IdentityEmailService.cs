namespace MedCorVis.Modules.Identity.Infrastructure.Services.Email;

using Microsoft.Extensions.Options;
using MedCorVis.Common.Configuration;
using MedCorVis.Common.Localization;
using MedCorVis.Common.Services.Email;
using MedCorVis.Modules.Identity.Application.Abstractions.Email;
using MedCorVis.Modules.Identity.Configuration;

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
    
    #region Send Email Confirmation
    
    public async Task SendConfirmationEmailAsync(
        Guid userId,
        string email,
        string fullName,
        string encodedToken,
        string culture,
        CancellationToken ct = default)
    {
        var confirmationLink = 
            $"{_frontendSettings.NormalizedBaseUrl}{_identityTokenSettings.NormalizedEmailConfirmationPath}" +
            $"?userId={userId}&token={encodedToken}";
        
        var translations = new ConfirmationEmailTranslations(
            Subject: _localizer.Get(TranslationKeys.EmailConfirmation.Subject, culture),
            Greeting: string.Format(_localizer
                .Get(TranslationKeys.EmailConfirmation.Greeting, culture), fullName),
            Instruction: _localizer.Get(TranslationKeys.EmailConfirmation.Instruction, culture),
            LinkLabel: _localizer.Get(TranslationKeys.EmailConfirmation.LinkLabel, culture),
            Expiry: string.Format(_localizer.Get(TranslationKeys.EmailConfirmation.Expiry, culture), 
                _identityTokenSettings.TokenExpirationInHours),
            Ignore: _localizer.Get(TranslationKeys.EmailConfirmation.Ignore, culture),
            Closing: _localizer.Get(TranslationKeys.EmailConfirmation.Closing, culture),
            AppName: _localizer.Get(TranslationKeys.AppGeneral.Name, culture));
        
        var message = new EmailMessage(
            To: email,
            Subject: translations.Subject,
            HtmlBody: BuildEmailConfirmationHtmlBody(confirmationLink, translations),
            PlainTextBody: BuildEmailConfirmationPlainTextBody(confirmationLink, translations));

        await _emailService.SendAsync(message, ct);
    }

    private static string BuildEmailConfirmationHtmlBody(
        string confirmationLink, ConfirmationEmailTranslations t)
    {
        return $"""
                <p>{t.Greeting}</p>
                <p>{t.Instruction}</p>
                <p><a href="{confirmationLink}">{t.LinkLabel}</a></p>
                <p>{t.Expiry}</p>
                <p>{t.Ignore}</p>
                <p>{t.Closing}</p>
                <p>{t.AppName}</p>
                """;
    }

    private static string BuildEmailConfirmationPlainTextBody(
        string confirmationLink, ConfirmationEmailTranslations t)
    {
        return $"""
                {t.Greeting}

                {t.Instruction}

                {confirmationLink}

                {t.Expiry}

                {t.Ignore}
                
                {t.Closing}
                
                {t.AppName}
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
    
    #endregion
    
    #region Send Password Reset Email
    
    public async Task SendPasswordResetEmailAsync(
        Guid userId,
        string email,
        string fullName,
        string encodedToken,
        string culture,
        CancellationToken ct = default)
    {
        var resetLink =
            $"{_frontendSettings.NormalizedBaseUrl}{_identityTokenSettings.NormalizedPasswordResetPath}" +
            $"?userId={userId}&token={encodedToken}";

        var translations = new PasswordResetEmailTranslations(
            Subject:     _localizer.Get(TranslationKeys.PasswordReset.Subject, culture),
            Greeting:    string.Format(_localizer.Get(TranslationKeys.PasswordReset.Greeting, culture), fullName),
            Instruction: _localizer.Get(TranslationKeys.PasswordReset.Instruction, culture),
            LinkLabel:   _localizer.Get(TranslationKeys.PasswordReset.LinkLabel, culture),
            Expiry:      string.Format(_localizer.Get(TranslationKeys.PasswordReset.Expiry, culture),
                         _identityTokenSettings.TokenExpirationInHours),
            Ignore:      _localizer.Get(TranslationKeys.PasswordReset.Ignore, culture),
            Closing:     _localizer.Get(TranslationKeys.PasswordReset.Closing, culture),
            AppName:     _localizer.Get(TranslationKeys.AppGeneral.Name, culture));

        var message = new EmailMessage(
            To:            email,
            Subject:       translations.Subject,
            HtmlBody:      BuildPasswordResetHtmlBody(resetLink, translations),
            PlainTextBody: BuildPasswordResetPlainTextBody(resetLink, translations));

        await _emailService.SendAsync(message, ct);
    }

    private static string BuildPasswordResetHtmlBody(string link, PasswordResetEmailTranslations t)
    {
        return $"""
                <p>{t.Greeting}</p>
                <p>{t.Instruction}</p>
                <p><a href="{link}">{t.LinkLabel}</a></p>
                <p>{t.Expiry}</p>
                <p>{t.Ignore}</p>
                <p>{t.Closing}</p>
                <p>{t.AppName}</p>
                """;
    }

    private static string BuildPasswordResetPlainTextBody(string link, PasswordResetEmailTranslations t)
    {
        return $"""
                {t.Greeting}

                {t.Instruction}

                {link}

                {t.Expiry}

                {t.Ignore}

                {t.Closing}

                {t.AppName}
                """;
    }

    private sealed record PasswordResetEmailTranslations(
        string Subject,
        string Greeting,
        string Instruction,
        string LinkLabel,
        string Expiry,
        string Ignore,
        string Closing,
        string AppName);
    
    #endregion
}