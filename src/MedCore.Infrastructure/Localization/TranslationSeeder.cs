namespace MedCore.Infrastructure.Localization;

using MedCore.Common.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

internal static class TranslationSeeder
{
    // Regional cultures (fr-CH, de-CH) are not seeded explicitly.
    // DbMessageLocalizer resolves them via the fallback chain: fr-CH → fr → en.
    // To override a regional culture, add an entry here with the specific culture key.
    private static readonly Dictionary<string, Dictionary<string, string>> Seeds = new()
    {
        [SupportedCultures.English] = new()
        {
            ["email.confirmation.subject"]     = "Confirm your email address",
            ["email.confirmation.greeting"]    = "Hi {0},",
            ["email.confirmation.instruction"] = "Thanks for registering. Please confirm your email address by clicking the link below.",
            ["email.confirmation.link_label"]  = "Confirm Email Address",
            ["email.confirmation.expiry"]      = "This link expires in {0} hours.",
            ["email.confirmation.ignore"]      = "If you did not create an account, you can safely ignore this email."
        },
        [SupportedCultures.French] = new()
        {
            ["email.confirmation.subject"]     = "Confirmez votre adresse e-mail",
            ["email.confirmation.greeting"]    = "Bonjour {0},",
            ["email.confirmation.instruction"] = "Merci de vous être inscrit. Veuillez confirmer votre adresse e-mail en cliquant sur le lien ci-dessous.",
            ["email.confirmation.link_label"]  = "Confirmer l'adresse e-mail",
            ["email.confirmation.expiry"]      = "Ce lien expire dans {0} heures.",
            ["email.confirmation.ignore"]      = "Si vous n'avez pas créé de compte, vous pouvez ignorer cet e-mail."
        },
        [SupportedCultures.German] = new()
        {
            ["email.confirmation.subject"]     = "Bestätigen Sie Ihre E-Mail-Adresse",
            ["email.confirmation.greeting"]    = "Hallo {0},",
            ["email.confirmation.instruction"] = "Vielen Dank für Ihre Registrierung. Bitte bestätigen Sie Ihre E-Mail-Adresse, indem Sie auf den folgenden Link klicken.",
            ["email.confirmation.link_label"]  = "E-Mail-Adresse bestätigen",
            ["email.confirmation.expiry"]      = "Dieser Link läuft in {0} Stunden ab.",
            ["email.confirmation.ignore"]      = "Wenn Sie kein Konto erstellt haben, können Sie diese E-Mail ignorieren."
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

        logger.LogInformation("Seeding translations...");

        var seeded = 0;
        var skipped = 0;

        foreach (var (culture, keys) in Seeds)
        {
            foreach (var (key, value) in keys)
            {
                if (await repository.ExistsAsync(culture, key))
                {
                    skipped++;
                    continue;
                }

                await repository.AddAsync(new Translation(culture, key, value));
                seeded++;
            }
        }
        
        if (seeded > 0) 
            await repository.SaveChangesAsync();

        logger.LogInformation(
            "Translation seeding complete. Seeded: {Seeded}, Skipped: {Skipped}.",
            seeded, skipped);
    }
}