using Autodissmark.ExternalServices.TextToSpeach.Contracts;
using Autodissmark.ExternalServices.WebDriverTaskBuilder;

namespace Autodissmark.ExternalServices.TextToSpeach.Voxworker;

public class TextToSpeach : ITextToSpeach
{
    private readonly IWebDriverTaskBuilder _webDriverTaskBuilder;


    public TextToSpeach(IWebDriverTaskBuilder webDriverTaskBuilder)
    {
        _webDriverTaskBuilder = webDriverTaskBuilder;
    }


    public async Task<byte[]?> GetAudioByText(string text)
    {
        var link = TextToSpeachSettings.Link;
        var defaultDelayInSeconds = TextToSpeachSettings.DefaultDelayInSeconds;

        var inputTextAreaXStr = TextToSpeachSettings.InputTextAreaXStr;
        var downloadButtonXStr = TextToSpeachSettings.DownloadButtonXStr;

        var downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), TextToSpeachSettings.BaseDownloadDirectory, $"{DateTime.Now.Ticks}");
        Directory.CreateDirectory(downloadDirectory);

        await _webDriverTaskBuilder.SetLink(link, defaultDelayInSeconds, downloadDirectory);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.ConsentBtnXStr);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.VoiceSelectXStr);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.VoiceOptionXStr);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.SpeedSelectXStr);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.SpeedOptionXStr);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.PitchSelectXStr);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.PitchOptionXStr);
        await _webDriverTaskBuilder.ClearInput(inputTextAreaXStr);
        await _webDriverTaskBuilder.InputText(inputTextAreaXStr, text);
        await _webDriverTaskBuilder.Click(downloadButtonXStr);
        await _webDriverTaskBuilder.WaitForDownladingFile(downloadDirectory, "*.mp3");
        var result = await _webDriverTaskBuilder.OutputFirstByPatternDownladedFile(downloadDirectory, ".mp3");

        return result;
    }
}