namespace Autodissmark.ExternalServices.TextToSpeach.Voxworker;

public static class TextToSpeachSettings
{
    public static string Link { get; private set; } = "https://voxworker.com/ru";
    public static string ConsentBtnXStr { get; private set; } = "/html/body/div[2]/div[2]/div[1]/div[2]/div[2]/button[1]";

    public static string VoiceSelectXStr { get; private set; } = "//*[@id=\"js-main-form\"]/ul[1]/li[1]/div/div[2]/b";
    public static string SpeedSelectXStr { get; private set; } = "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[2]/b";
    public static string PitchSelectXStr { get; private set; } = "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[2]/b";

    public static string DownloadButtonXStr { get; private set; } = "//*[@id=\"js-download-button\"]";
    public static string InputTextAreaXStr { get; private set; } = "//*[@id=\"js-textarea\"]";

    
    private static Dictionary<string, string> artistNamesXStrs = new Dictionary<string, string>()
    {
        { "Олег", "//*[@id=\"js-main-form\"]/ul[1]/li[1]/div/div[3]/div/ul/ul[2]/li[2]" },
        { "Михаил", "//*[@id=\"js-main-form\"]/ul[1]/li[1]/div/div[3]/div/ul/ul[2]/li[3]" },
        { "Анна", "//*[@id=\"js-main-form\"]/ul[1]/li[1]/div/div[3]/div/ul/ul[2]/li[4]" },
        { "Мария", "//*[@id=\"js-main-form\"]/ul[1]/li[1]/div/div[3]/div/ul/ul[2]/li[5]" },
        { "Елена", "//*[@id=\"js-main-form\"]/ul[1]/li[1]/div/div[3]/div/ul/ul[2]/li[6]" }
    };
    public static Dictionary<string, string> ArtistNamesXStrs { get => artistNamesXStrs; }

    private static Dictionary<double, string> voiceSpeedsXStrs = new Dictionary<double, string>()
    {
        { 0.5, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[1]" },
        { 0.6, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[2]" },
        { 0.7, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[3]" },
        { 0.8, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[4]" },
        { 0.9, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[5]" },
        { 1.0, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[6]" },
        { 1.1, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[7]" },
        { 1.2, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[8]" },
        { 1.3, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[9]" },
        { 1.4, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[10]" },
        { 1.5, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[11]" },
        { 2.0, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[12]" },
        { 3.0, "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[13]" },
    };
    public static Dictionary<double, string> VoiceSpeedsXStrs { get => voiceSpeedsXStrs; }

    private static Dictionary<double, string> voicePitchXStrs = new Dictionary<double, string>()
    {
        { 1.8, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[1]" },
        { 1.7, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[2]" },
        { 1.6, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[3]" },
        { 1.5, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[4]" },
        { 1.4, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[5]" },
        { 1.3, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[6]" },
        { 1.2, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[7]" },
        { 1.1, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[8]" },
        { 1.0, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[9]" },
        { 0.9, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[10]" },
        { 0.8, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[11]" },
        { 0.7, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[12]" },
        { 0.6, "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[13]" },
    };

    public static Dictionary<double, string> VoicePitchXStrs { get => voicePitchXStrs; }

    public static int DefaultDelayInSeconds { get; private set; } = 10;
    
    
}