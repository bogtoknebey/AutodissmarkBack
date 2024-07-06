using Autodissmark.TGBot.Options;
using System.Text.Json;

namespace Autodissmark.TGBot;

public class JsonReader
{
    public const string JsonFilePath = "config.json";

    public JsonReader()
    {
    }

    public async Task Read() 
    {
        if (File.Exists(JsonFilePath))
        {
            try
            {
                var jsonContent = await File.ReadAllTextAsync(JsonFilePath);

                var autogenerationOptions = JsonSerializer.Deserialize<AutogenerationOptions>(
                    JsonDocument.Parse(jsonContent).RootElement.GetProperty("Autogeneration").GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                var telegramDataOptions = JsonSerializer.Deserialize<TelegramDataOptions>(
                    JsonDocument.Parse(jsonContent).RootElement.GetProperty("Telegram").GetProperty("Data").GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                var telegramRolesOptions = JsonSerializer.Deserialize<TelegramRolesOptions>(
                    JsonDocument.Parse(jsonContent).RootElement.GetProperty("Telegram").GetProperty("Roles").GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                // Вывод значений на консоль
                Console.WriteLine("Autogeneration Options:");
                Console.WriteLine($"LinesCounts: {string.Join(", ", autogenerationOptions.LinesCounts)}");
                Console.WriteLine($"WordsInLineCounts: {string.Join(", ", autogenerationOptions.WordsInLineCounts)}");
                Console.WriteLine($"SwitchLanguages: {string.Join(", ", autogenerationOptions.SwitchLanguages)}");
                Console.WriteLine($"SwitchTimes: {string.Join(", ", autogenerationOptions.SwitchTimes)}");
                Console.WriteLine($"Targets: {string.Join(", ", autogenerationOptions.Targets)}");
                Console.WriteLine($"Voices: {string.Join(", ", autogenerationOptions.Voices)}");
                Console.WriteLine($"Beats: {string.Join(", ", autogenerationOptions.Beats)}");

                Console.WriteLine("\nTelegram Data Options:");
                Console.WriteLine($"LogFilePath: {telegramDataOptions.LogFilePath}");
                Console.WriteLine($"UserChatsFolderPath: {telegramDataOptions.UserChatsFolderPath}");

                Console.WriteLine("\nTelegram Roles Options:");
                Console.WriteLine($"User Attempts: {telegramRolesOptions.User.Attempts}");
                Console.WriteLine($"Admin Attempts: {telegramRolesOptions.Admin.Attempts}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading or deserializing JSON file: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine("JSON file not found.");
        }
    }
}
