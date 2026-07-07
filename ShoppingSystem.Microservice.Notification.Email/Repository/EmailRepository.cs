using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Domain.Entities;
using ShoppingSystem.Microservice.Notification.Email.Settings;

namespace ShoppingSystem.Microservice.Notification.Email.Repository;

public class EmailRepository : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailRepository
        (IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task SendAsync
        (EmailMessage message, CancellationToken ct)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(
            _settings.DisplayName,
            _settings.From));

        email.To.Add(MailboxAddress.Parse(message.To));

        email.Subject = message.Subject;

        email.Body = new TextPart(message.IsHtml ? "html" : "plain")
        {
            Text = message.Body
        };

        using var smtp = new SmtpClient();

        var secureSocketOption = SecureSocketOptions.None;

        await smtp.ConnectAsync(
            _settings.Host,
            _settings.Port,
            secureSocketOption,
            ct);

        await smtp.AuthenticateAsync(
            _settings.UserName,
            _settings.Password,
            ct);

        await smtp.SendAsync(email, ct);

        await smtp.DisconnectAsync(true, ct);
    }
}