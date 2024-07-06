using Autodissmark.Domain.Enums;

namespace Autodissmark.Application.Login.DTO;

public record LoginOutputDTO
(
    int AuthorId,
    Role Role
);
