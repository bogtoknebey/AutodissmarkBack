using Autodissmark.TGBot.Autogeneration;
using Autodissmark.TGBot.Options;
using System.Reflection;
using System.Text.Json;

namespace Autodissmark.TGBot;

public class TgBotBuilder
{
    private const string ConfigFileName = "tgBotSettings.json";

    private ApiOptions _apiOptions;
    private AutogenerationOptions _autogenerationOptions;
    private TelegramOptions _telegramOptions;

    public TgBotBuilder()
    {
    }

    public void SetupOptions()
    {
        var libraryPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var configFilePath = Path.Combine(libraryPath, ConfigFileName);

        var jsonString = File.ReadAllText(configFilePath);
        var configRoot = JsonDocument.Parse(jsonString).RootElement;

        var apiConfig = configRoot.GetProperty(ApiOptions.SectionName).GetRawText();
        _apiOptions = JsonSerializer.Deserialize<ApiOptions>(apiConfig);

        var autogenerationConfig = configRoot.GetProperty(AutogenerationOptions.SectionName).GetRawText();
        _autogenerationOptions = JsonSerializer.Deserialize<AutogenerationOptions>(autogenerationConfig);

        var telegramConfig = configRoot.GetProperty(TelegramOptions.SectionName).GetRawText();
        _telegramOptions = JsonSerializer.Deserialize<TelegramOptions>(telegramConfig);
    }

    public void ViewOptions() 
    {
        Console.WriteLine($"_apiOptions.BaseUrl: {_apiOptions.BaseUrl}");
        Console.WriteLine($"_autogenerationOptions.SwitchLanguages.FirstOrDefault(): {_autogenerationOptions.SwitchLanguages.FirstOrDefault()}");
        Console.WriteLine($"_telegramOptions.BotKey: {_telegramOptions.BotKey}");
    }

    public async Task<bool> RunAsync(CancellationToken ct)
    {
        SetupOptions();
        AutogenerationLogic autogenerationLogic = new AutogenerationLogic(_apiOptions, _autogenerationOptions);
        return await autogenerationLogic.Autogenerate();
    }
}
