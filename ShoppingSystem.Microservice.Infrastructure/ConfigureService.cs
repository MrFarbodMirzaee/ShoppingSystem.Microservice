using Infrastructure.Contexts;
using Infrastructure.Factories;
using Infrastructure.Persistence.Contracts.Base;
using Infrastructure.Persistence.Options;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using ShoppingSystem.Microservice.Application.Attributes;
using ShoppingSystem.Microservice.Application.Services;

namespace Infrastructure;

public static class ConfigureService
{
    public static IServiceCollection RegisterInfrastructureDependencies
    (this IServiceCollection services, 
    IConfiguration config)
    {
        #region RegisterServices
        services.Scan(scan => scan
            .FromAssemblyOf<PaymentRepository>()
            .AddClasses(classes => classes
                .Where(type => 
                    type.GetCustomAttributes(typeof(ScopedService), false)
                        .Any()))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        #endregion
        
        #region AppDbContextSettings
        services.AddDbContext<AppDbContext>(options =>
        {
        });
        services.AddScoped<AppDbContext>(sp =>
        {
            var option = sp.GetRequiredService<Option>();
            return new AppDbContext(UnitOfWorkBase.CreateOptions(option),option);
        });
        
        services.AddSingleton(
            OptionFactory.Create(config));
        #endregion

        #region RabbitMqSettings

        services.AddSingleton<IConnection>(sp =>
        {
            var factory = new RabbitMQ.Client.ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });
        
        services.AddSingleton<IMessageReceiverService
            , MessageReceiverServiceRepository>();

        #endregion

        return services;
    }
}