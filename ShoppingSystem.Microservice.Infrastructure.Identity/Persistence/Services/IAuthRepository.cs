
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;

namespace ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Services;

public interface IAuthRepository : IUserQueryService
{
    Task<ApplicationUser?> GetUserByEmailAsync(string email);

    Task<ApplicationUser?> GetUserByGoogleIdAsync(string googleId);

    Task CreateUserAsync(ApplicationUser user);

    Task SaveChangesAsync();
}