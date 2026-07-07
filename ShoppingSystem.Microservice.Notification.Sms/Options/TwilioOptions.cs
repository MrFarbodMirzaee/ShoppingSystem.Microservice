namespace ShoppingSystem.Microservice.Notification.Sms.Options;

public class TwilioOptions
{
    public const string SectionName = "Twilio";

    public string AccountSid { get; set; } = null!;

    public string AuthToken { get; set; } = null!;

    public string FromNumber { get; set; } = null!;
}