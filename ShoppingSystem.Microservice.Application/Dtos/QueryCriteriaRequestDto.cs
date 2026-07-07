namespace ShoppingSystem.Microservice.Application.Dtos;

public record QueryCriteriaRequestDto
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}