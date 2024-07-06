namespace Autodissmark.Domain.Options;

public class JwtOptions
{
    public const string SectionName = "JwtOptions";
    public bool ValidateIssuer { get; set; }
    public bool ValidateAudience { get; set; }
    public bool ValidateLifetime { get; set; }
    public bool ValidateIssuerSigningKey { get; set; }
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
