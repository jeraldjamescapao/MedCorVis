namespace MedCore.Common.Localization;

public interface IMessageLocalizer
{
    string Get(string key, string culture);
}