namespace Autodissmark.API.Responses;

public record GetRandomTextResponse
(
    int Id,
    string Text,
    string Title,
    DateTime AddedDate
);
