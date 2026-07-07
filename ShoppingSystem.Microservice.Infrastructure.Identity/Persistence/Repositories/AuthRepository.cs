using Microsoft.EntityFrameworkCore;
using ShoppingSystem.Microservice.Infrastructure.Identity.Context;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Entities;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Services;

namespace ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Repositories;

public class AuthRepository
(IdentityAppDbContext identityContext) : IAuthRepository
{
    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await identityContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync
            (x => x.Email == email);
    }

    public async Task<ApplicationUser?> GetUserByGoogleIdAsync(string googleId)
    {
        return await identityContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync
            (x => x.GoogleId == googleId);
    }

    public async Task CreateUserAsync(ApplicationUser user)
    {
        identityContext.Users.Add(user);
    }

    public async Task SaveChangesAsync()
    {
        await identityContext.SaveChangesAsync();
    }

    public async Task<string?> GetEmailByIdAsync(Guid userId, CancellationToken ct)
    {
        var user = await identityContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync
                (x => x.Id == userId);
        
        return user?.Email;
    }

    public async Task<bool> UserExistsAsync(Guid userId, CancellationToken ct)
    {
        return await identityContext.Users
            .AsNoTracking()
            .AnyAsync(x => x.Id == userId, ct);
    }
}