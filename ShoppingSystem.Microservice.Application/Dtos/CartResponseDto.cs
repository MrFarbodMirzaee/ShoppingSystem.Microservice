namespace ShoppingSystem.Microservice.Application.Dtos;

public record CartResponseDto(
    Guid Id,
    Guid CustomerId,
    IReadOnlyCollection<CartItemResponseDto> CartItems,
    decimal TotalPrice
);