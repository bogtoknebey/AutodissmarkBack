namespace Autodissmark.TGBot.API.Requests;

public record CreateAutoVoiceoverRequest
(
    int textId,
    int voiceId
);
