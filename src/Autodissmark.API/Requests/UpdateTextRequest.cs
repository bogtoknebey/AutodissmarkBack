namespace Autodissmark.API.Requests;

public record UpdateTextRequest
(
    int Id,
    int AuthorId,
    string Text,
    string Title
);