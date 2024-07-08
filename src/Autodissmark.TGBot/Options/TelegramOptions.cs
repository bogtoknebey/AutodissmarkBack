namespace Autodissmark.TGBot.Options;

public class TelegramDataOptions
{
    public string LogFilePath { get; set; }
    public string UserChatsFolderPath { get; set; }
}

public class TelegramRolesOptions
{
    public RoleOptions User { get; set; }
    public RoleOptions Admin { get; set; }
}

public class RoleOptions
{
    public int Attempts { get; set; }
}

public class TelegramOptions
{
    public const string SectionName = "Telegram";
    public string BotKey { get; set; }
    public TelegramDataOptions Data { get; set; }
    public TelegramRolesOptions Roles { get; set; }
}
