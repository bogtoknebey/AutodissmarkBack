namespace Autodissmark.TGBot.Options;

public class TelegramRolesOptions
{
    public RoleOptions User { get; set; }
    public RoleOptions Admin { get; set; }
}

public class RoleOptions
{
    public int Attempts { get; set; }
}