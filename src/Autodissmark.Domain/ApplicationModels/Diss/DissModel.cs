namespace Autodissmark.Domain.ApplicationModels.Diss;

public class DissModel
{
    public int Id { get; set; }
    public int BeatId { get; set; }
    public string URI { get; set; }
    public string Target { get; set; }

    public ICollection<DissAcapellaModel> DissAcapellas { get; set; }

    public static DissModel Create(int beatId, string URI, string target, ICollection<DissAcapellaModel> dissAcapellas)
    {
        return new DissModel()
        {
            BeatId = beatId,
            URI = URI,
            Target = target,
            DissAcapellas = dissAcapellas
        };
    }
}
