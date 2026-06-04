namespace MedCorVis.Modules.Identity.Configuration;

using System.ComponentModel.DataAnnotations;

internal sealed class IdentityTokenSettings
{
    public const string SectionName = "IdentityTokens";
    
    /// <summary>Use <see cref="NormalizedEmailConfirmationPath"/> when building email confirmation paths.</summary>
    [Required] public string EmailConfirmationPath { get; init; } = null!;
    
    public string NormalizedEmailConfirmationPath => "/" + EmailConfirmationPath.TrimStart('/');
    
    /// <summary>Use <see cref="NormalizedPasswordResetPath"/> when building password reset paths.</summary>
    [Required] public string PasswordResetPath { get; init; } = null!;

    public string NormalizedPasswordResetPath => "/" + PasswordResetPath.TrimStart('/');
    
    [Range(1, int.MaxValue)] public int TokenExpirationInHours { get; init; }
}