namespace ShoppingSystem.Microservice.Application.Dtos;

public record PaymentResponseDto(
    Guid Id,
    Guid OrderId,
    decimal Amount,
    string Currency,
    string? TransactionId,
    string Status,
    DateTimeOffset? PaidAt
);