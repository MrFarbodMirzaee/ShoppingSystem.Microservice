using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.User;

namespace ShoppingSystem.Microservice.Application.UseCases.Identity.User.Notifications;

public class UserRegisteredEventHandler 
    : INotificationHandler<UserRegisteredEvent>
{
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;

    public UserRegisteredEventHandler(
        IEmailService emailService,
        ISmsService smsService)
    {
        _emailService = emailService;
        _smsService = smsService;
    }
    
    public async Task Handle(UserRegisteredEvent notification, CancellationToken ct)
    {
        var body = $"<h1>Welcome {notification.Name}</h1>";

        await _emailService.SendAsync(
            new EmailMessage
            {
                To = notification.Email,
                Subject = "Welcome to Shopping System",
                Body = body,
                IsHtml = true
            },
            ct);
        
        
        //ToDo: send it for users
        
        // await _smsService.SendAsync(
        //     new SmsRequestDto
        //     {
        //         PhoneNumber = notification.PhoneNumber,
        //         Message = $"Welcome {notification.Name}! Thank you for registering."
        //     },
        //     ct);
    }
}