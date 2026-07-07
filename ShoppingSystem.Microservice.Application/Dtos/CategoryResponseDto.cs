namespace ShoppingSystem.Microservice.Application.Dtos;

public record CategoryResponseDto(
    Guid Id,
    string Name,
    string Description,
    Guid? ParentCategoryId,
    bool IsActive
);