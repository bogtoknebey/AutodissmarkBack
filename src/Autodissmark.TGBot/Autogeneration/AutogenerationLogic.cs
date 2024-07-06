namespace Autodissmark.TGBot.Autogeneration;

public class AutogenerationLogic
{
    // private readonly _autogenerationConfigs
    private readonly HttpClient _httpClient;

    public AutogenerationLogic()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetGeneratedTextAsync(string endpointUrl)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(endpointUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<byte[]> GetAudioFileAsync(string endpointUrl)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(endpointUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsByteArrayAsync();
    }
}
