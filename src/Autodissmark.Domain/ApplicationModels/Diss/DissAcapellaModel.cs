namespace Autodissmark.Domain.ApplicationModels.Diss;

public class DissAcapellaModel
{
    public int Id { get; set; }
    public int DissId { get; set; }
    public int AcapellaId { get; set; }
    public int StartPointMilliseconds { get; set; }

    public static DissAcapellaModel Create(int acapellaId, int startPointMilliseconds)
    {
        return new DissAcapellaModel()
        {
            AcapellaId = acapellaId,
            StartPointMilliseconds = startPointMilliseconds
        };
    }
}
