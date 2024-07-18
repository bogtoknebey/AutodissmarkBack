using Autodissmark.ExternalServices.TextToSpeach.Contracts;
using Autodissmark.ExternalServices.TextToSpeach.DTO;
using System.Text;
using System.Text.Json;

namespace Autodissmark.ExternalServices.TextToSpeach.ApihostAPI;

public class TextToSpeach : ITextToSpeach
{
    private const string Url = "https://apihost.ru/tts.php";
    private const int CharLimit = 975;
    private readonly Dictionary<string, int> _artistIdByName = new Dictionary<string, int>()
    {
        { "Костя",  3 },
        { "Робот",  2070 }
    };

    private HttpClient _client { get; set; }

    public TextToSpeach()
    {
        _client = new HttpClient();
    }

    private string PrepareText(string nativeText)
    {
        var preparedText = string.Join(' ', nativeText.Split('\r', '\n'));
        preparedText = preparedText.Substring(0, Math.Min(CharLimit, preparedText.Length));

        return preparedText;
    }

    private async Task<string> GetAudioUrl(string url, string jsonData)
    {
        StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using JsonDocument doc = JsonDocument.Parse(json);
        var audioFileUrl = doc.RootElement.GetProperty("audio").GetString();

        return audioFileUrl;
    }

    private async Task<byte[]> DownloadAudioFileAsync(string url)
    {
        HttpResponseMessage response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<byte[]?> GetAudioByText(GetAudioByTextDTO dto)
    {
        string jsonData = "" +
            "{\"data\":[{" +
                "\"lang\":\"ru-RU\"," +
                $"\"speaker\":\"{_artistIdByName[dto.ArtistName]}\"," +
                "\"emotion\":\"good\"," +
                $"\"text\":\"{PrepareText(dto.Text)}\"," +
                $"\"rate\":\"1.1\"," +
                $"\"pitch\":\"0.8\"," +
                "\"type\":\"wav\"," +
                "\"pause\":\"0\"" +
            "}]}";

        var audioFileUrl = await GetAudioUrl(Url, jsonData);
        var audioFile = await DownloadAudioFileAsync(audioFileUrl);

        return audioFile;
    }
}
