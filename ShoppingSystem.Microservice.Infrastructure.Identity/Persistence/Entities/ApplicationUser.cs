using Microsoft.AspNetCore.Identity;

namespace ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public bool IsActive { get; set; } = true;
    
    public string? GoogleId { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; }
        = new List<RefreshToken>();
}