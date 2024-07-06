namespace Autodissmark.Domain.ApplicationModels;

public class VoiceModel
{
    public int Id { get; set; }
    public int ArtistId { get; set; }
    public double Speed { get; set; }
    public double Pitch { get; set; }

    public ArtistModel ArtistModel { get; set; }
}
