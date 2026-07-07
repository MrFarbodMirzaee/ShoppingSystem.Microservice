using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ShoppingSystem.Microservice.Application;

public static class ConfigureService
{
    public static IServiceCollection RegisterApplicationDependencies
    (this IServiceCollection services)
    {
        var assembly = 
        typeof(ConfigureService).Assembly;
        
        #region MediatR
        services.AddMediatR(config =>
        config.RegisterServicesFromAssemblies(assembly));
        #endregion
        
        #region FluentValidation
        services
        .AddValidatorsFromAssembly(assembly);
        #endregion
        
        #region AutoMapper
        services
        .AddAutoMapper(cfg => { }, assembly);
        #endregion
        
        return services;
    }
}