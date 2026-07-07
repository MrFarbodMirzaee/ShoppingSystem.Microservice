namespace ShoppingSystem.Microservice.Application.Dtos;

public record CartItemResponseDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);