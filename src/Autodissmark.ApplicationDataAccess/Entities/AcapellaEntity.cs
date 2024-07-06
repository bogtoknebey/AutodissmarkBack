namespace Autodissmark.ApplicationDataAccess.Entities;

public class AcapellaEntity
{
    public AcapellaEntity()
    {
        DissAcapellaEntities = new List<DissAcapellaEntity>();
    }

    public int Id { get; set; }
    public int TextEntityId { get; set; }
    public int? VoiceEntityId { get; set; }
    public int DurationMilliseconds { get; set; }
    public int StartDelayMilliseconds { get; set; }
    public int EndDelayMilliseconds { get; set; }
    public string URI { get; set; }

    public virtual TextEntity TextEntity { get; set; }
    public virtual VoiceEntity? VoiceEntity { get; set; }
    public virtual ICollection<DissAcapellaEntity> DissAcapellaEntities { get; set; }
}
