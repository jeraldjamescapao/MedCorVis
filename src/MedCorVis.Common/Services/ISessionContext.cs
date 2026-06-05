namespace MedCorVis.Common.Services;

public interface ISessionContext
{
    string? IpAddress { get; }
    string? UserAgent { get; }
}