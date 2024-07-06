namespace Autodissmark.API.Requests;

public record CreateDissRequest
(
    int AcapellaId,
    int BeatId,
    int StartPointMilliseconds,
    string Target
);
