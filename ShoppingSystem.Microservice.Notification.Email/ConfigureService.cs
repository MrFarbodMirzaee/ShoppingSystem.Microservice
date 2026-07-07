using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Notification.Email.Repository;
using ShoppingSystem.Microservice.Notification.Email.Settings;

namespace ShoppingSystem.Microservice.Notification.Email;

public static class ConfigureService
{
    public static IServiceCollection RegisterEmailDependencies
    (this IServiceCollection services
    ,IConfiguration con)
    {
      #region EmailSettings
      
      
      services.Configure<EmailSettings>(
          con.GetSection("EmailSettings"));

      services.AddScoped
          <IEmailService, EmailRepository>();
      #endregion
        
        return services;
    }
}