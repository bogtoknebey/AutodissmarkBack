namespace Autodissmark.ApplicationDataAccess.Entities;

public class DissAcapellaEntity
{
    public int Id { get; set; }
    public int DissEntityId { get; set; }
    public int AcapellaEntityId { get; set; }
    public int StartPointMilliseconds { get; set; }

    public virtual DissEntity DissEntity { get; set; }
    public virtual AcapellaEntity AcapellaEntity { get; set; }
}
