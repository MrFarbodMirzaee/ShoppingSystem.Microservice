using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;

namespace ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Services;

public interface ITokenService
{
    Task<string> CreateTokenAsync(ApplicationUser user);
    Task<string> GenerateRefreshToken();
}