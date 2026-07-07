namespace ShoppingSystem.Microservice.Application.Dtos;

public record OrderResponseDto(
    Guid Id,
    Guid UserId,
    string OrderNumber,
    decimal TotalPrice,
    string Currency,
    string Status,
    DateTimeOffset CreatedAt,
    IReadOnlyCollection<OrderItemResponseDto> Items
);