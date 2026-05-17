namespace MedCorVis.Modules.Identity.Application.Contracts.Authentication.Responses;

using System.Text.Json.Serialization;

public sealed record RefreshResponse(string AccessToken)
{
    [JsonIgnore]
    public string RawRefreshToken { get; init; } = string.Empty;
    [JsonIgnore]
    public DateTimeOffset RefreshTokenExpiresAt { get; init; }
};