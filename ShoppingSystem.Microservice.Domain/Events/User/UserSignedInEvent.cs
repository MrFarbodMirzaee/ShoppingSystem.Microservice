using MediatR;

namespace ShoppingSystem.Microservice.Domain.Events.User;

/// <summary>
/// Represents a domain event that occurs when a user successfully signs in.
/// This event can be published through MediatR to notify other parts of the system
/// about the user's authentication activity.
/// </summary>
/// <param name="Email">The email address of the signed-in user.</param>
/// <param name="FirstName">The first name of the signed-in user.</param>
public record UserSignedInEvent(
    string Email,
    string FirstName
) : INotification;