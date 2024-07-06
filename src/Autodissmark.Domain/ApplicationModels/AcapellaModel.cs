namespace Autodissmark.Domain.ApplicationModels;

public class AcapellaModel
{
    public int Id { get; set; }
    public int TextEntityId { get; set; }
    public int? VoiceEntityId { get; set; }
    public int DurationMilliseconds { get; set; }
    public int StartDelayMilliseconds { get; set; }
    public int EndDelayMilliseconds { get; set; }
    public string URI { get; set; }

    public static AcapellaModel Create(int textEntityId, int? voiceEntityId, int durationMilliseconds, int startDelayMilliseconds, int endDelayMilliseconds, string URI)
    {
        return new AcapellaModel()
        {
            TextEntityId = textEntityId,
            VoiceEntityId = voiceEntityId,
            DurationMilliseconds = durationMilliseconds,
            StartDelayMilliseconds = startDelayMilliseconds,
            EndDelayMilliseconds = endDelayMilliseconds,
            URI = URI
        };
    }
}
