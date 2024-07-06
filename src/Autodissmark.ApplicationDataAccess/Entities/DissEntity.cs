namespace Autodissmark.ApplicationDataAccess.Entities;

public class DissEntity
{
    public DissEntity()
    {
        DissAcapellaEntities = new List<DissAcapellaEntity>();
    }

    public int Id { get; set; }
    public int BeatEntityId { get; set; }
    public string URI { get; set; }
    public string Target { get; set; }

    public virtual BeatEntity BeatEntity { get; set; }
    public virtual ICollection<DissAcapellaEntity> DissAcapellaEntities { get; set; }
}
