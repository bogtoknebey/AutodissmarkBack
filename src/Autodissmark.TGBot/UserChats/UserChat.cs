namespace Autodissmark.TGBot.Models;

public enum TgRole
{
    Admin = 1,
    User = 2
}

public class UserChat
{
    public TgRole Role { get; set; }
    public bool Disable { get; set; }
    public List<int> UnimportantMessages { get; set; } = new List<int>();
    public int LeftAttempts { get; set; }
    public DateTime LastModify { get; set; } 
    public string SelectedTarget { get; set; }
    public int SelectedBeatId { get; set; }
    public int SelectedVoiceId { get; set; }
}
