using ShoppingSystem.Microservice.Domain.Entities;

namespace ShoppingSystem.Microservice.Application.Services;

public interface IEmailService
{
    Task SendAsync(EmailMessage message, CancellationToken ct);
}