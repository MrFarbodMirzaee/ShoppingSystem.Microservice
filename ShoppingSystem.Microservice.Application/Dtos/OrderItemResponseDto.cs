namespace ShoppingSystem.Microservice.Application.Dtos;

public sealed record OrderItemResponseDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    string Currency,
    int Quantity
);