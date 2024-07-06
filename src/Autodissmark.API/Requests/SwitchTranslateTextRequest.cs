using Autodissmark.ExternalServices.Translate.GoogleTranslate;

namespace Autodissmark.API.Requests;

public record SwitchTranslateTextRequest
(
    string Text,
    Language SwitchLanguage, 
    int SwitchTimes
);