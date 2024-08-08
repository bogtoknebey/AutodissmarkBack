using Autodissmark.Application.Voiceover.CommonVoiceover.DTO;

namespace Autodissmark.Application.Voiceover.CommonVoiceover;

public interface ICommonVoiceoverLogic
{
    Task<GetVoiceoverDTO> GetVoiceoverById(int id, CancellationToken ct);
    Task<ICollection<GetVoiceoverDTO>> GetAllVoiceovers(int textId, CancellationToken ct);
    Task<ICollection<GetVoiceoverDTO>> GetVoiceoversPage(int textId, int pageSize, int pageNumber, CancellationToken ct);
    Task DeleteVoiceover(int id, CancellationToken ct);
}
