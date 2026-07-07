namespace ShoppingSystem.Microservice.Application.Dtos;

public record AddressResponseDto(
    Guid Id,
    string Street,
    string City,
    string State,
    string Country,
    string PostalCode
);