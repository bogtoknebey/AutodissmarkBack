using Autodissmark.Domain.Enums;

namespace Autodissmark.Application.Author.DTO;

public record CreateAuthorInputDTO
(
    string Name,
    string Email,
    string Password,
    Role Role
);
