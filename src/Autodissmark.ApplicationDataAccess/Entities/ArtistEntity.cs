namespace Autodissmark.ApplicationDataAccess.Entities;

public class ArtistEntity
{
    public ArtistEntity()
    {
        VoiceEntities = new List<VoiceEntity>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Source { get; set; }

    public virtual ICollection<VoiceEntity> VoiceEntities { get; set; }
}
