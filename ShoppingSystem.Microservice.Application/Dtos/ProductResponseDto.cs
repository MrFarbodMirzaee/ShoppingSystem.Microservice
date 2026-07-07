namespace ShoppingSystem.Microservice.Application.Dtos;

public record ProductResponseDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    Guid CategoryId,
    bool IsAvailable,
    IReadOnlyCollection<ProductImageResponseDto> Images
);