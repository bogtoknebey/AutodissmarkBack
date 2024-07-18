using Autodissmark.ExternalServices.Translate.GoogleTranslate;

namespace Autodissmark.TGBot.TgSettings.Options;

public class OptionRow
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class AutogenerationOptions
{
    public const string SectionName = "Autogeneration";
    public List<int> LinesCounts { get; set; }
    public List<int> WordsInLineCounts { get; set; }
    public List<Language> SwitchLanguages { get; set; }
    public List<int> SwitchTimes { get; set; }
    public List<string> Targets { get; set; }
    public List<OptionRow> Voices { get; set; }
    public List<OptionRow> Beats { get; set; }
}
