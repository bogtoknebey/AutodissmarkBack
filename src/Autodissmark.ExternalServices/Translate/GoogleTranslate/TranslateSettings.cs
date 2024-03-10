namespace Autodissmark.ExternalServices.Translate.GoogleTranslate;

public static class TranslateSettings
{
    public static string GetLink(Language fromLang, Language toLang)
        => $"https://translate.google.com.ua/?sl={_languageCodes[fromLang]}&tl={_languageCodes[toLang]}";

    public static string ConsentBtnXStr = "/html/body/c-wiz/div/div/div/div[2]/div[1]/div[3]/div[1]/div[1]/form[2]/div/div/button/div[3]";
    public static string InputTextAreaXStr = "/html/body/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[2]/div[2]/c-wiz[1]/span/span/div/textarea";
    public static string SwitchBtnXStr = "/html/body/c-wiz/div/div[2]/c-wiz/div[2]/c-wiz/div[1]/div[1]/c-wiz/div[1]/c-wiz/div[3]/div/span/button";
    private static Dictionary<Language, string> _languageCodes = new Dictionary<Language, string>()
    {
        [Language.Russian] = "ru",
        [Language.English] = "en",
        [Language.Chinese] = "zh-TW",
    };

    public static int DefaultDelayInSeconds = 10;
}

public enum Language
{
    Russian,
    English,
    Chinese
}
