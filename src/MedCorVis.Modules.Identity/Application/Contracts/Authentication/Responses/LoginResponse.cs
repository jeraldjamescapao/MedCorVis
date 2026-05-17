namespace MedCorVis.Modules.Identity.Application.Contracts.Authentication.Responses;

using System.Text.Json.Serialization;

public sealed record LoginResponse(
    Guid UserId,
    string Email,
    string FullName,
    string Culture,
    IList<string> Roles,
    string AccessToken)
{
    [JsonIgnore]
    public string RawRefreshToken { get; init; } = string.Empty;
    [JsonIgnore]
    public DateTimeOffset RefreshTokenExpiresAt { get; init; }
};