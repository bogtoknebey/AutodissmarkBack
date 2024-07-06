namespace Autodissmark.ApplicationDataAccess.Entities;

public class VoiceEntity
{
    public VoiceEntity()
    {
        AcapellaEntities = new List<AcapellaEntity>();
    }

    public int Id { get; set; }
    public int ArtistEntityId { get; set; }
    public double Speed { get; set; }
    public double Pitch { get; set; }

    public virtual ArtistEntity ArtistEntity { get; set; }
    public virtual ICollection<AcapellaEntity> AcapellaEntities { get; set; }
}
