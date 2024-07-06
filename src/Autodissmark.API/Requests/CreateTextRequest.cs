namespace Autodissmark.API.Requests;

public record CreateTextRequest
(
    int AuthorId,
    string Text,
    string Title
);