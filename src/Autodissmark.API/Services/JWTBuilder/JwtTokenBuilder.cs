using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Autodissmark.Core.Constants;
using Autodissmark.Domain.Enums;

namespace Autodissmark.API.Services.JWTBuilder;

public class JwtTokenBuilder : IJwtTokenBuilder
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _key;
    private readonly List<Claim> _claims;

    private int _expirationDateDays = 180;

    public JwtTokenBuilder(string issuer, string audience, string key)
    {
        _issuer = issuer;
        _audience = audience;
        _key = key;
        _claims = new List<Claim>();
    }

    public IJwtTokenBuilder AddUserIdClaim(int id)
    {
        _claims.Add(new Claim(ClaimConstants.UserId, id.ToString()));
        return this;
    }

    public IJwtTokenBuilder AddRoleClaim(Role role)
    {
        _claims.Add(new Claim(ClaimConstants.Role, role.ToString()));
        return this;
    }

    public IJwtTokenBuilder AddExpirationDateDays(int expirationDateDays)
    {
        _expirationDateDays = expirationDateDays;
        return this;
    }

    public string Build()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _issuer,
            Audience = _audience,
            Subject = new ClaimsIdentity(_claims),
            Expires = DateTime.UtcNow.AddDays(_expirationDateDays),
            SigningCredentials = credentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
