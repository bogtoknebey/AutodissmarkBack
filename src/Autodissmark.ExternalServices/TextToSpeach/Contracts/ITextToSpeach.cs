using Autodissmark.ExternalServices.TextToSpeach.DTO;

namespace Autodissmark.ExternalServices.TextToSpeach.Contracts;

public interface ITextToSpeach
{
    public Task<byte[]?> GetAudioByText(GetAudioByTextDTO dto);
}
