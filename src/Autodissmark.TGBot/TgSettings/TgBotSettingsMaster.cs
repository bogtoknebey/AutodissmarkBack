using Autodissmark.TGBot.TgSettings.Options;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Autodissmark.TGBot.TgSettings;

public class TgBotSettingsMaster
{
    private const string ConfigFileName = "tgBotSettings.json";
    private readonly string _configFilePath;

    public TgBotSettingsMaster()
    {
        var libraryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        _configFilePath = Path.Combine(libraryPath, ConfigFileName);
    }

    private async Task<string> ReadSection(string sectionName)
    {
        var jsonString = await File.ReadAllTextAsync(_configFilePath);
        var configRoot = JsonDocument.Parse(jsonString).RootElement;

        return configRoot.GetProperty(sectionName).GetRawText();
    }

    public async Task<ApiOptions> ReadApiOptions()
    {
        var section = await ReadSection(ApiOptions.SectionName);
        return JsonSerializer.Deserialize<ApiOptions>(section);
    }

    public async Task<AutogenerationOptions> ReadAutogenerationOptions()
    {
        var section = await ReadSection(AutogenerationOptions.SectionName);

        var options = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
        
        return JsonSerializer.Deserialize<AutogenerationOptions>(section, options);
    }

    public async Task<TelegramOptions> ReadTelegramOptions()
    {
        var section = await ReadSection(TelegramOptions.SectionName);
        return JsonSerializer.Deserialize<TelegramOptions>(section);
    }

    public async Task WriteAutogenerationOptions(AutogenerationOptions options)
    {
        var jsonString = await File.ReadAllTextAsync(_configFilePath);
        var jsonObject = JsonNode.Parse(jsonString) as JsonObject;

        var optionsJson = JsonSerializer.Serialize(options);
        var optionsNode = JsonNode.Parse(optionsJson);

        jsonObject[AutogenerationOptions.SectionName] = optionsNode;
        await File.WriteAllTextAsync(_configFilePath, jsonObject.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    }
}
