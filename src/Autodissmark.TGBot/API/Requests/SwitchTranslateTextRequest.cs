using Autodissmark.ExternalServices.Translate.GoogleTranslate;

namespace Autodissmark.TGBot.API.Requests;

public record SwitchTranslateTextRequest
(
    string Text,
    Language SwitchLanguage, 
    int SwitchTimes
);