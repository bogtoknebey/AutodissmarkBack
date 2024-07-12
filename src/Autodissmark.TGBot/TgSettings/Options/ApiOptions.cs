namespace Autodissmark.TGBot.TgSettings.Options;

public class ApiOptions
{
    public const string SectionName = "Api";

    public int AuthorId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public string BaseUrl { get; set; }

    public string Login { get; set; }
    public string GenerateText { get; set; }
    public string AddTargetToText { get; set; }
    public string SwitchTranslateText { get; set; }
    public string CreateText { get; set; }
    public string CreateAutoVoiceover { get; set; }
    public string CreateDiss { get; set; }
    public string GetDiss { get; set; }

}
