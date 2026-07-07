using MediatR;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Domain.Events.Product;

namespace ShoppingSystem.Microservice.Application.UseCases.Product.Notifications;

public class ProductPriceChangedNotificationHandler
    : INotificationHandler<ProductPriceChangedEvent>
{
    private readonly IEmailService _emailService;

    public ProductPriceChangedNotificationHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(ProductPriceChangedEvent notification, CancellationToken ct)
    {
        var body = $@"
            <h2>Product Price Updated</h2>
            <p>The price of a product has been changed.</p>

            <p><b>Product ID:</b> {notification.ProductId}</p>
            <p><b>Product Name:</b> {notification.Name}</p>
            <p><b>Old Price:</b> {notification.OldPrice:C}</p>
            <p><b>New Price:</b> {notification.NewPrice:C}</p>
        ";

        await _emailService.SendAsync(
            new EmailMessage
            {
                To = notification.Email.Value,
                Subject = "Product Price Updated",
                Body = body,
                IsHtml = true,
            },
            ct);
    }
}