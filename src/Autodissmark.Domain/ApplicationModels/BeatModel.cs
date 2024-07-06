namespace Autodissmark.Domain.ApplicationModels;

public class BeatModel()
{
    public int Id { get; set; }
    public string URI { get; set; }
    public string? SourceLink { get; set; }
    public int? BPM { get; set; }

    public static BeatModel Create(string URI, string? sourceLink = null, int? BPM = null)
    {
        return new BeatModel()
        {
            URI = URI,
            SourceLink = sourceLink,
            BPM = BPM
        };
    }
}