using Autodissmark.Domain.Enums;

namespace Autodissmark.API.Services.JWTBuilder;

public interface IJwtTokenBuilder
{
    IJwtTokenBuilder AddUserIdClaim(int id);
    IJwtTokenBuilder AddRoleClaim(Role role);
    IJwtTokenBuilder AddExpirationDateDays(int expirationDateDays);
    string Build();
}
