namespace Autodissmark.Application.Text.DTO;

public record UpdateTextInputDTO
(
    int Id,
    int AuthorId,
    string Text,
    string Title
);
