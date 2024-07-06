namespace Autodissmark.API.Requests;

public record CreateAutoVoiceoverRequest
(
    int textId,
    int voiceId
);
