namespace Autodissmark.TGBot.Options;

public class AutogenerationOptions
{
    public List<int> LinesCounts { get; set; }
    public List<int> WordsInLineCounts { get; set; }
    public List<string> SwitchLanguages { get; set; }
    public List<int> SwitchTimes { get; set; }
    public List<string> Targets { get; set; }
    public List<int> Voices { get; set; }
    public List<int> Beats { get; set; }
}
