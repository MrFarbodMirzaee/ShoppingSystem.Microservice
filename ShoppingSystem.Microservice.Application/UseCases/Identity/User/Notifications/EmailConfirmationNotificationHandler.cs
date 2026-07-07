using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.User;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.User.Notifications;

public class EmailConfirmationNotificationHandler
    : INotificationHandler<EmailConfirmationRequestedEvent>
{
    private readonly IEmailService _emailService;

    public EmailConfirmationNotificationHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(
        EmailConfirmationRequestedEvent notification,
        CancellationToken ct)
    {
        var confirmationLink =
            $"https://your-app.com/confirm-email?email={notification.Email}&token={notification.ConfirmationToken}";

        var body = $@"
            <h2>Confirm your email</h2>
            <p>Hello {notification.FirstName},</p>
            <p>Please confirm your email by clicking the link below:</p>
            <a href='{confirmationLink}'>Confirm Email</a>
        ";

        await _emailService.SendAsync(
            new EmailMessage
            {
                To = notification.Email,
                Subject = "Email Confirmation",
                Body = body,
                IsHtml = true
            },
            ct);
    }
}