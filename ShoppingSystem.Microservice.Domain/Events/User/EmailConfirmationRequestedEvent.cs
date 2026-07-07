using MediatR;

namespace ShoppingSystem.Microservice.Domain.Events.User;

/// <summary>
/// Represents a domain event that occurs when a user requests email confirmation.
/// This event contains the information required to send a confirmation email
/// and verify the user's email address.
/// </summary>
/// <param name="Email">The email address of the user requesting confirmation.</param>
/// <param name="ConfirmationToken">The token used to confirm the user's email address.</param>
/// <param name="FirstName">The user's first name used for personalization.</param>
public record EmailConfirmationRequestedEvent(
    string Email,
    string ConfirmationToken,
    string FirstName
) : INotification;