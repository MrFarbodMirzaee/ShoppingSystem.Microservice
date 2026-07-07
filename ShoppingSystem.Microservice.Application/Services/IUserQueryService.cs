namespace ShoppingSystem.Microservice.Application.Services;

public interface IUserQueryService
{
    Task<string?> GetEmailByIdAsync
        (Guid userId, CancellationToken ct);
    Task<bool> UserExistsAsync
        (Guid userId, CancellationToken ct);
}