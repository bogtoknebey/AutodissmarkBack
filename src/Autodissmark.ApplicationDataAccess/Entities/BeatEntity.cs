namespace Autodissmark.ApplicationDataAccess.Entities;

public class BeatEntity
{
    public BeatEntity()
    {
        DissEntities = new List<DissEntity>();
    }

    public int Id { get; set; }
    public string URI { get; set; }
    public string? SourceLink { get; set; }
    public int? BPM { get; set; }

    public virtual ICollection<DissEntity> DissEntities { get; set; }
}
