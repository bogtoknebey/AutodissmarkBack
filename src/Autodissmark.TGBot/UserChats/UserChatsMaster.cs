using Autodissmark.TGBot.Models;
using System.Text.Json;

namespace Autodissmark.TGBot.UserChats;

public class UserChatsMaster
{
    private const string LogFilePath = "../../data/telegram/log/log.txt";
    private const string ChatsFolderPath = "../../data/telegram/userChats";

    private string ChatFilePath(long chatId) => $"{ChatsFolderPath}/{chatId}.txt";

    public async Task<Dictionary<long, UserChat>> ReadChatsStates()
    {
        var chats = new Dictionary<long, UserChat>();
        string[] chatFilePaths = Directory.GetFiles(ChatsFolderPath, "*.txt");

        foreach (var chatFilePath in chatFilePaths)
        {
            var chatId = Convert.ToInt64(Path.GetFileNameWithoutExtension(chatFilePath));

            var json = await File.ReadAllTextAsync(chatFilePath);
            var chat = JsonSerializer.Deserialize<UserChat>(json);

            if (chat is not null)
            {
                chats.Add(chatId, chat);
            }
        }

        return chats;
    }

    public async Task<bool> WriteChatsStates(Dictionary<long, UserChat> chats)
    {
        foreach (var chat in chats)
        {
            var chatFilePath = ChatFilePath(chat.Key);

            var json = JsonSerializer.Serialize(chat.Value, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(chatFilePath, json);
        }
        return true;
    }

    public async Task<bool> AddLog(string logText)
    {
        string text = File.ReadAllText(LogFilePath);
        text += $"{logText}\n\n";
        File.WriteAllText(LogFilePath, text);
        return true;
    }
}
