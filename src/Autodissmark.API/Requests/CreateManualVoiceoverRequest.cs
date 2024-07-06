namespace Autodissmark.API.Requests;

public record CreateManualVoiceoverRequest
(
    int TextId,
    IFormFile AudioData
);
