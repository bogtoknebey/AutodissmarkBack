using Autodissmark.ExternalServices.Translate.Contracts;
using Autodissmark.ExternalServices.WebDriverTaskBuilder;
using Core.Extentions;

namespace Autodissmark.ExternalServices.Translate.GoogleTranslate;

public class Translator : ITranslate
{
    private const int SwitchDelay = 500;

    private readonly IWebDriverTaskBuilder _webDriverTaskBuilder;

    public Translator(IWebDriverTaskBuilder webDriverTaskBuilder)
    {
        _webDriverTaskBuilder = webDriverTaskBuilder;
    }

    public async Task<string> GetText(string text, int switchTimes, Language fromLang, Language toLang)
    {
        var defaultDelayInSeconds = TranslateSettings.DefaultDelayInSeconds;
        var link = TranslateSettings.GetLink(fromLang, toLang);

        var consentBtnXPath = TranslateSettings.ConsentBtnXStr;
        var inputTextAreaXPath = TranslateSettings.InputTextAreaXStr;
        var outputTextAreaXPath = inputTextAreaXPath;
        var switchBtn = TranslateSettings.SwitchBtnXStr;

        await _webDriverTaskBuilder.SetLink(link, defaultDelayInSeconds);
        await _webDriverTaskBuilder.Click(consentBtnXPath);
        await _webDriverTaskBuilder.InputText(inputTextAreaXPath, text, switchBtn, 500);
        await _webDriverTaskBuilder.Click(switchBtn, SwitchDelay, switchTimes);
        var result = await _webDriverTaskBuilder.OutputText(outputTextAreaXPath, 3000, switchBtn, 500);

        if (result.ComputeCyrillicPercentage() < 0.5)
        {
            await _webDriverTaskBuilder.SetLink(link, defaultDelayInSeconds);
            await _webDriverTaskBuilder.Click(consentBtnXPath);
            await _webDriverTaskBuilder.InputText(inputTextAreaXPath, result, switchBtn, 500);
            await _webDriverTaskBuilder.Click(switchBtn, SwitchDelay, 1);
            result = await _webDriverTaskBuilder.OutputText(outputTextAreaXPath, 3000, switchBtn, 500); ;
        }

        return result;
    }
}
