#nullable disable
namespace ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Options;

public class JwtOptions
{
    public string Issuer { get; set; } 
    public string Audience { get; set; }
    public string Key { get; set; }
    public int ExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}