using Autodissmark.Domain.Options;
using Autodissmark.ExternalServices.TextToSpeach.Contracts;
using Autodissmark.ExternalServices.TextToSpeach.DTO;
using Autodissmark.ExternalServices.WebDriverTaskBuilder;
using Microsoft.Extensions.Options;


namespace Autodissmark.ExternalServices.TextToSpeach.Voxworker;

public class TextToSpeach : ITextToSpeach
{
    private readonly string _tempPath;
    private readonly IWebDriverTaskBuilder _webDriverTaskBuilder;

    public TextToSpeach(
        IOptions<FilePathOptions> filePathOptions,
        IWebDriverTaskBuilder webDriverTaskBuilder)
    {
        _tempPath = filePathOptions.Value.TempPath;
        _webDriverTaskBuilder = webDriverTaskBuilder;
    }

    public async Task<byte[]?> GetAudioByText(GetAudioByTextDTO dto)
    {
        var link = TextToSpeachSettings.Link;
        var defaultDelayInSeconds = TextToSpeachSettings.DefaultDelayInSeconds;

        var inputTextAreaXStr = TextToSpeachSettings.InputTextAreaXStr;
        var downloadButtonXStr = TextToSpeachSettings.DownloadButtonXStr;

        var downloadDirectory = Path.Combine(_tempPath, $"{DateTime.Now.Ticks}");
        Directory.CreateDirectory(downloadDirectory);

        await _webDriverTaskBuilder.SetLink(link, defaultDelayInSeconds, downloadDirectory);
        await _webDriverTaskBuilder.ClickIfThere(TextToSpeachSettings.ConsentBtnXStr);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.VoiceSelectXStr);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.ArtistNamesXStrs[dto.ArtistName]);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.SpeedSelectXStr);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.VoiceSpeedsXStrs[dto.VoiceSpeed]);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.PitchSelectXStr);
        await _webDriverTaskBuilder.Click(TextToSpeachSettings.VoicePitchXStrs[dto.VoicePitch]);
        await _webDriverTaskBuilder.ClearInput(inputTextAreaXStr);
        await _webDriverTaskBuilder.InputText(inputTextAreaXStr, dto.Text);
        await _webDriverTaskBuilder.Click(downloadButtonXStr);
        await _webDriverTaskBuilder.WaitForDownladingFile(downloadDirectory, "*.mp3");
        var result = await _webDriverTaskBuilder.OutputFirstByPatternDownladedFile(downloadDirectory, ".mp3");

        return result;
    }
}