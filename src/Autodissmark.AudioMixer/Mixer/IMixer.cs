using Autodissmark.AudioMixer.Mixer.DTO;

namespace Autodissmark.AudioMixer.Mixer;

public interface IMixer
{
    Task MixFiles(BaseMixFileDTO baseMixFile, MixFileDTO additionalMixFile, string outputFilePath, CancellationToken ct);
}
