namespace ShoppingSystem.Microservice.Application.Dtos;

public record InventoryResponseDto(
    Guid Id,
    Guid ProductId,
    byte Quantity,
    string Status
);