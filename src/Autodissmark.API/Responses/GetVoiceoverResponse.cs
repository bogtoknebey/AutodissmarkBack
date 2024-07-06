namespace Autodissmark.API.Responses;

public record GetVoiceoverResponse
(
    int Id,
    byte[] AudioData
);