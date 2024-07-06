using Autodissmark.Application.Voiceover.CommonVoiceover.DTO;

namespace Autodissmark.Application.Voiceover.CommonVoiceover;

public interface ICommonVoiceoverLogic
{
    Task<GetVoiceoverDTO> GetVoiceoverById(int id, CancellationToken ct);
    Task<ICollection<GetVoiceoverDTO>> GetVoiceoversByTextId(int textId, CancellationToken ct);
    Task DeleteVoiceover(int id, CancellationToken ct);
}
