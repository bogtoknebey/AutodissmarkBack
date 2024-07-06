using Microsoft.AspNetCore.Http;

namespace Autodissmark.Application.Voiceover.ManualVoiceover.DTO;

public record CreateManualVoiceoverDTO
(
    int TextId,
    IFormFile AudioData
);
