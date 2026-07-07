namespace ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }

    public string Token { get; set; } 

    public DateTimeOffset ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public DateTimeOffset? RevokedAt { get; set; }

    public Guid UserId { get; set; }
    
    public bool IsActive =>
        RevokedAt == null && DateTime.UtcNow < ExpiresAt;

    public ApplicationUser User { get; set; }
}