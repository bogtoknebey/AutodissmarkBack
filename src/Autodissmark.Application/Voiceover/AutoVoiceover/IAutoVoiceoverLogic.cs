using Autodissmark.Application.Voiceover.AutoVoiceover.DTO;

namespace Autodissmark.Application.Voiceover.AutoVoiceover;

public interface IAutoVoiceoverLogic
{
    Task<int> CreateAutoVoiceover(CreateAutoVoiceoverDTO dto, CancellationToken ct);
}
