namespace Autodissmark.ExternalServices.TextToSpeach.Voxworker;

public static class TextToSpeachSettings
{
    public static string Link { get; private set; } = "https://voxworker.com/ru";
    public static string BaseDownloadDirectory { get; private set; } = "data\\Downloads";

    public static string ConsentBtnXStr { get; private set; } = "/html/body/div[2]/div[2]/div[1]/div[2]/div[2]/button[1]";
    public static string VoiceSelectXStr { get; private set; } = "//*[@id=\"js-main-form\"]/ul[1]/li[1]/div/div[2]/b";
    public static string VoiceOptionXStr { get; private set; } = "//*[@id=\"js-main-form\"]/ul[1]/li[1]/div/div[3]/div/ul/ul[2]/li[3]";
    public static string SpeedSelectXStr { get; private set; } = "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[2]/b";
    public static string SpeedOptionXStr { get; private set; } = "//*[@id=\"js-main-form\"]/ul[1]/li[2]/div/div[3]/div/ul/li[6]";
    public static string PitchSelectXStr { get; private set; } = "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[2]/b";
    public static string PitchOptionXStr { get; private set; } = "//*[@id=\"js-main-form\"]/ul[1]/li[3]/div/div[3]/div/ul/li[10]";

    public static string DownloadButtonXStr { get; private set; } = "//*[@id=\"js-download-button\"]";

    public static string InputTextAreaXStr { get; private set; } = "//*[@id=\"js-textarea\"]";

    public static int DefaultDelayInSeconds { get; private set; } = 10;
}