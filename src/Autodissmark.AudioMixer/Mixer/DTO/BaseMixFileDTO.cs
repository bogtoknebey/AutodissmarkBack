namespace Autodissmark.AudioMixer.Mixer.DTO;

public record BaseMixFileDTO
(
    string FilePath,
    float Volume // 0.00f - 1.00f
);
