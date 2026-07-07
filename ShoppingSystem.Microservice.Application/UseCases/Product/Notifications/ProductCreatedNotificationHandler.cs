using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.Product;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Notifications;

public class ProductCreatedNotificationHandler
    : INotificationHandler<ProductCreatedEvent>
{
    private readonly IEmailService _emailService;

    public ProductCreatedNotificationHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken ct)
    {
        var body = $@"
            <h2>New Product Added</h2>
            <p>A new product has been added to the system.</p>

            <p><b>Product Name:</b> {notification.Name}</p>
            <p><b>Price:</b> {notification.Price}</p>
            <p><b>Product ID:</b> {notification.ProductId}</p>

            <br/>
            <p>Check it out in the store!</p>
        ";
        
        foreach (var email in notification.AdminEmails)
        {
            await _emailService.SendAsync(
                new EmailMessage
                {
                    To = email,
                    Subject = "New Product Created",
                    Body = body,
                    IsHtml = true
                },
                ct);
        }

       
    }
}