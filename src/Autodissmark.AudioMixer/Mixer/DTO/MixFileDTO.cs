namespace Autodissmark.AudioMixer.Mixer.DTO;

public record MixFileDTO
(
    string FilePath,
    float Volume // 0.00f - 1.00f
    // TODO: add start delay
);
