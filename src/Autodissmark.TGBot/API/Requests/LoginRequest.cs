namespace Autodissmark.TGBot.API.Requests;

public record LoginRequest
(
    string Email,
    string Password
);