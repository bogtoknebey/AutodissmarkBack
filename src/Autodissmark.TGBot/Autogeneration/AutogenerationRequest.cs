using Autodissmark.ExternalServices.Translate.GoogleTranslate;

namespace Autodissmark.TGBot.Autogeneration;

public class AutogenerationRequest
{
    public int? LinesCount { get; set; }
    public int? WordsInLineCount { get; set; }
    public Language? SwitchLanguage { get; set; }
    public int? SwitchTimes { get; set; }
    public string? Target { get; set; }
    public int? VoiceId { get; set; }
    public int? BeatId { get; set; }
}
