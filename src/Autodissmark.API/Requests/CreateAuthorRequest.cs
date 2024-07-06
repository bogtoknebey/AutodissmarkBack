namespace Autodissmark.API.Requests;

public record CreateAuthorRequest
(
    string Name,
    string Email,
    string Password,
    string Role
);
