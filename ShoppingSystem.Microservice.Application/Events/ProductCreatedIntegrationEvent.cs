namespace ShoppingSystem.Microservice.Application.Events;

public sealed record ProductCreatedIntegrationEvent(
    Guid ProductId,
    string Name,
    decimal Price,
    string Currency);