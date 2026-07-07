using MediatR;

namespace ShoppingSystem.Microservice.Domain.Events.User;

/// <summary>
/// Represents a domain event that occurs when a new user is registered.
/// This event can be published through MediatR to notify other parts of the system
/// about the newly registered user.
/// </summary>
/// <param name="Email">The email address of the registered user.</param>
/// <param name="Name">The name of the registered user.</param>
public record UserRegisteredEvent(
    string Email,
    string Name
) : INotification;