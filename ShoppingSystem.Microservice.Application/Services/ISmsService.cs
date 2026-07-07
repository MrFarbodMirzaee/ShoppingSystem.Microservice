using ShoppingSystem.Microservice.Application.Dtos;

namespace ShoppingSystem.Microservice.Application.Services;

public interface ISmsService
{
    Task SendAsync(SmsRequestDto request , CancellationToken ct);
}