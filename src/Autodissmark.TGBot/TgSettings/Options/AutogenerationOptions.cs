using Autodissmark.ExternalServices.Translate.GoogleTranslate;

namespace Autodissmark.TGBot.TgSettings.Options;

public class AutogenerationOptions
{
    public const string SectionName = "Autogeneration";
    public List<int> LinesCounts { get; set; }
    public List<int> WordsInLineCounts { get; set; }
    public List<Language> SwitchLanguages { get; set; }
    public List<int> SwitchTimes { get; set; }
    public List<string> Targets { get; set; }
    public List<int> Voices { get; set; }
    public List<int> Beats { get; set; }
}
