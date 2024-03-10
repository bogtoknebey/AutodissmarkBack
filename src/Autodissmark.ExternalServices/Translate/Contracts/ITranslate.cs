using Autodissmark.ExternalServices.Translate.GoogleTranslate;

namespace Autodissmark.ExternalServices.Translate.Contracts;

public interface ITranslate
{
    Task<string> GetText(string text, int switchTimes, Language fromLang, Language toLang);
}
