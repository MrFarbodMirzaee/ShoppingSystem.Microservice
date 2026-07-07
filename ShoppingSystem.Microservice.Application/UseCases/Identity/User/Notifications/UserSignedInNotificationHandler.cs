using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.User;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.User.Notifications;

public class UserSignedInNotificationHandler 
    : INotificationHandler<UserSignedInEvent>
{
    private readonly IEmailService _emailService;

    public UserSignedInNotificationHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(UserSignedInEvent notification, CancellationToken ct)
    {
        var body = $@"
            <h2>Login Alert</h2>
            <p>Hello {notification.FirstName},</p>
            <p>You just signed in to your account.</p>
            <p>If this was not you, please reset your password immediately.</p>
        ";

        await _emailService.SendAsync(
            new EmailMessage
            {
                To = notification.Email,
                Subject = "New Login Detected",
                Body = body,
                IsHtml = true
            },
            ct);
    }
}