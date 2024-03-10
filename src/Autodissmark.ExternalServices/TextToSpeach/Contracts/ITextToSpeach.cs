namespace Autodissmark.ExternalServices.TextToSpeach.Contracts;

public interface ITextToSpeach
{
    public Task<byte[]?> GetAudioByText(string text);
}
