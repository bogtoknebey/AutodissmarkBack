namespace Autodissmark.Application.Text.DTO;

public record CreateTextInputDTO
(
    int AuthorId,
    string Text, 
    string Title
);