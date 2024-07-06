namespace Autodissmark.ExternalServices.TextToSpeach.DTO;

public record GetAudioByTextDTO
(
    string Text,
    string ArtistName, 
    double VoiceSpeed, 
    double VoicePitch
);