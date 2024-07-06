using Autodissmark.Application.Voiceover.ManualVoiceover.DTO;

namespace Autodissmark.Application.Voiceover.ManualVoiceover;

public interface IManualVoiceoverLogic
{
    Task<int> CreateManualVoiceover(CreateManualVoiceoverDTO dto, CancellationToken ct);
}
