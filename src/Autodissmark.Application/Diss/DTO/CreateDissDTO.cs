namespace Autodissmark.Application.Diss.DTO;

public record CreateDissDTO
(
    int AcapellaId,
    int BeatId,
    int StartPointMilliseconds,
    string Target
);
