namespace MedCorVis.Modules.Localization.Infrastructure.Persistence.Seeds;

using MedCorVis.Common.Authorization;
using MedCorVis.Common.Localization;
using MedCorVis.Modules.Localization.Application.Abstractions;
using MedCorVis.Modules.Localization.Infrastructure.Persistence.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

internal static class TranslationSeeder
{
    // Regional cultures (fr-CH, de-CH) are not seeded explicitly.
    // DbMessageLocalizer resolves them via the fallback chain: fr-CH → fr → en.
    // To override a regional culture, add an entry here with the specific culture key.
    private static readonly Dictionary<string, Dictionary<string, string>> Seeds 
        = new(StringComparer.OrdinalIgnoreCase)
    {
        [SupportedCultures.English] = new(StringComparer.OrdinalIgnoreCase)
        {
            [TranslationKeys.AppGeneral.Name] = "MedCorVis by Jerald James Capao",
            [TranslationKeys.EmailConfirmation.Subject]     = "Confirm your email address",
            [TranslationKeys.EmailConfirmation.Greeting]    = "Hi {0},",
            [TranslationKeys.EmailConfirmation.Instruction] = "Welcome! To activate your account, please confirm your email address by clicking the link below.",
            [TranslationKeys.EmailConfirmation.LinkLabel]   = "Confirm Email Address",
            [TranslationKeys.EmailConfirmation.Expiry]      = "This link expires in {0} hours.",
            [TranslationKeys.EmailConfirmation.Ignore]      = "If you did not create an account, you can safely ignore this email.",
            [TranslationKeys.EmailConfirmation.Closing]     = "Yours Truly,",
            [TranslationKeys.PasswordReset.Subject]     = "Reset your password",
            [TranslationKeys.PasswordReset.Greeting]    = "Hi {0},",
            [TranslationKeys.PasswordReset.Instruction] = "We received a request to reset your password. Click the link below to set a new one.",
            [TranslationKeys.PasswordReset.LinkLabel]   = "Reset Password",
            [TranslationKeys.PasswordReset.Expiry]      = "This link expires in {0} hours.",
            [TranslationKeys.PasswordReset.Ignore]      = "If you did not request a password reset, you can safely ignore this email.",
            [TranslationKeys.PasswordReset.Closing]     = "Yours Truly,"
        },
        [SupportedCultures.French] = new(StringComparer.OrdinalIgnoreCase)
        {
            [TranslationKeys.EmailConfirmation.Subject]     = "Confirmez votre adresse e-mail",
            [TranslationKeys.EmailConfirmation.Greeting]    = "Bonjour {0},",
            [TranslationKeys.EmailConfirmation.Instruction] = "Bienvenue ! Pour activer votre compte, veuillez confirmer votre adresse e-mail en cliquant sur le lien ci-dessous.",
            [TranslationKeys.EmailConfirmation.LinkLabel]   = "Confirmer l'adresse e-mail",
            [TranslationKeys.EmailConfirmation.Expiry]      = "Ce lien expire dans {0} heures.",
            [TranslationKeys.EmailConfirmation.Ignore]      = "Si vous n'avez pas créé de compte, vous pouvez ignorer cet e-mail.",
            [TranslationKeys.EmailConfirmation.Closing]     = "Cordialement,",
            [TranslationKeys.PasswordReset.Subject]     = "Réinitialisez votre mot de passe",
            [TranslationKeys.PasswordReset.Greeting]    = "Bonjour {0},",
            [TranslationKeys.PasswordReset.Instruction] = "Nous avons reçu une demande de réinitialisation de votre mot de passe. Cliquez sur le lien ci-dessous pour en définir un nouveau.",
            [TranslationKeys.PasswordReset.LinkLabel]   = "Réinitialiser le mot de passe",
            [TranslationKeys.PasswordReset.Expiry]      = "Ce lien expire dans {0} heures.",
            [TranslationKeys.PasswordReset.Ignore]      = "Si vous n'avez pas demandé de réinitialisation, vous pouvez ignorer cet e-mail.",
            [TranslationKeys.PasswordReset.Closing]     = "Cordialement,"
        },
        [SupportedCultures.German] = new(StringComparer.OrdinalIgnoreCase)
        {
            [TranslationKeys.EmailConfirmation.Subject]     = "Bestätigen Sie Ihre E-Mail-Adresse",
            [TranslationKeys.EmailConfirmation.Greeting]    = "Hallo {0},",
            [TranslationKeys.EmailConfirmation.Instruction] = "Willkommen! Um Ihr Konto zu aktivieren, bestätigen Sie bitte Ihre E-Mail-Adresse, indem Sie auf den folgenden Link klicken.",
            [TranslationKeys.EmailConfirmation.LinkLabel]   = "E-Mail-Adresse bestätigen",
            [TranslationKeys.EmailConfirmation.Expiry]      = "Dieser Link läuft in {0} Stunden ab.",
            [TranslationKeys.EmailConfirmation.Ignore]      = "Wenn Sie kein Konto erstellt haben, können Sie diese E-Mail ignorieren.",
            [TranslationKeys.EmailConfirmation.Closing]     = "Mit freundlichen Grüßen,",
            [TranslationKeys.PasswordReset.Subject]     = "Setzen Sie Ihr Passwort zurück",
            [TranslationKeys.PasswordReset.Greeting]    = "Hallo {0},",
            [TranslationKeys.PasswordReset.Instruction] = "Wir haben eine Anfrage zum Zurücksetzen Ihres Passworts erhalten. Klicken Sie auf den folgenden Link, um ein neues Passwort festzulegen.",
            [TranslationKeys.PasswordReset.LinkLabel]   = "Passwort zurücksetzen",
            [TranslationKeys.PasswordReset.Expiry]      = "Dieser Link läuft in {0} Stunden ab.",
            [TranslationKeys.PasswordReset.Ignore]      = "Wenn Sie keine Passwortzurücksetzung angefordert haben, können Sie diese E-Mail ignorieren.",
            [TranslationKeys.PasswordReset.Closing]     = "Mit freundlichen Grüßen,",
        }
    };
    
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var repository = scope.ServiceProvider
            .GetRequiredService<ITranslationRepository>();
        
        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger(nameof(TranslationSeeder));

        TranslationSeederLogMessages.TranslationSeedingStarted(logger, null);

        var existingKeys = await repository.GetAllKeysAsync();
        
        var seeded = 0;
        var skipped = 0;

        foreach (var (culture, keys) in Seeds)
        {
            foreach (var (key, value) in keys)
            {
                if (existingKeys.Contains((culture, key)))
                {
                    skipped++;
                    continue;
                }

                await repository.AddAsync(
                    culture, 
                    key, 
                    value, 
                    null, 
                    SystemActors.System, 
                    true);
                
                seeded++;
            }
        }
        
        if (seeded > 0) 
            await repository.SaveChangesAsync();

        TranslationSeederLogMessages.TranslationSeedingCompleted(logger, seeded, skipped, null);
    }
}