using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Notification.Sms.Options;
using ShoppingSystem.Microservice.Notification.Sms.Repository;

namespace ShoppingSystem.Microservice.Notification.Sms;

public static class ConfigureService
{
    public static IServiceCollection RegisterSmsNotificationDependencies(
        this IServiceCollection services,
        IConfiguration con)
    {
        services.Configure<TwilioOptions>(
            con.GetSection(TwilioOptions.SectionName));
        
        services.AddScoped<ISmsService>(_ =>
            new SmsRepository(
                con["Twilio:AccountSid"]!,
                con["Twilio:AuthToken"]!,
                con["Twilio:FromNumber"]!
            ));

        return services;
    }
}