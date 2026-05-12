namespace MedCore.Modules.Localization.Domain;

using MedCore.Common.Auditing;
using MedCore.Common.Exceptions;
using MedCore.Common.Localization;

internal sealed class Translation : IAuditableEntity
{
    public const int KeyMaxLength = 200;
    public const int CultureMaxLength = 10;
    
    public long Id { get; private set; }
    public string Culture { get; private set; } = null!;
    public string Key { get; private set; } = null!;
    public string Value { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsSystemDefined { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public string CreatedBy { get; private set; } = null!;
    public DateTimeOffset? ModifiedAtUtc { get; private set; }
    public string? ModifiedBy { get; private set; }

    private Translation() { }

    private Translation(
        string culture, 
        string key, 
        string value, 
        string createdBy, 
        string? description = null,
        bool isSystemDefined = false)
    {
        Culture = culture;
        Key = key;
        Value = value;
        Description = description;
        IsActive = true;
        IsSystemDefined = isSystemDefined;
        CreatedAtUtc = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }
    
    public static Translation Create(
        string culture,
        string key,
        string value,
        string createdBy,
        string? description = null,
        bool isSystemDefined = false)
    {
        if (string.IsNullOrWhiteSpace(culture))
            throw new DomainException("DOMAIN_TRANSLATION_INVALID_CULTURE", "Culture is required.");
        if (string.IsNullOrWhiteSpace(key))
            throw new DomainException("DOMAIN_TRANSLATION_INVALID_KEY", "Key is required.");
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("DOMAIN_TRANSLATION_INVALID_VALUE", "Value is required.");
        
        var trimmedCulture = culture.Trim();
        var trimmedKey = key.Trim();
        var trimmedValue = value.Trim();
        
        if (!SupportedCultures.All.Contains(trimmedCulture))
            throw new DomainException("DOMAIN_TRANSLATION_INVALID_CULTURE", "Unsupported culture.");
        if (trimmedKey.Length > KeyMaxLength)
            throw new DomainException("DOMAIN_TRANSLATION_INVALID_KEY", $"Key cannot exceed {KeyMaxLength} characters.");
        
        if (string.IsNullOrWhiteSpace(createdBy))
            throw new DomainException("DOMAIN_TRANSLATION_INVALID_CREATED_BY", "CreatedBy is required.");

        return new Translation(
            trimmedCulture,
            trimmedKey,
            trimmedValue,
            createdBy,
            string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            isSystemDefined);
    }
    
    public void Update(string value, string? description, string modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("DOMAIN_TRANSLATION_INVALID_VALUE", "Value is required.");
        if (string.IsNullOrWhiteSpace(modifiedBy))
            throw new DomainException("DOMAIN_TRANSLATION_INVALID_MODIFIED_BY", "ModifiedBy is required.");

        var trimmedValue = value.Trim();
        var trimmedDescription = string.IsNullOrWhiteSpace(description) ? null : description.Trim();

        if (trimmedValue == Value && trimmedDescription == Description) return;

        Value = trimmedValue;
        Description = trimmedDescription;
        ModifiedAtUtc = DateTimeOffset.UtcNow;
        ModifiedBy = modifiedBy;
    }
    
    public void Deactivate(string modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(modifiedBy))
            throw new DomainException("DOMAIN_TRANSLATION_INVALID_MODIFIED_BY", "ModifiedBy is required.");

        if (!IsActive) return;

        IsActive = false;
        ModifiedAtUtc = DateTimeOffset.UtcNow;
        ModifiedBy = modifiedBy;
    }
}