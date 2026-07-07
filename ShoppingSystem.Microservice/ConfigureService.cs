using System.Text.Json.Serialization;
using Asp.Versioning;
using Infrastructure.Messaging;
using Infrastructure.Persistence.HealthChecks;
using Microsoft.OpenApi;
using Serilog;
using ShoppingSystem.Microservice.Application.Services;
using ShoppingSystem.Microservice.Application.UseCases.Inventory.Consumers;
using ShoppingSystem.Microservice.BackgroundServices;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.HealthChecks;
using ShoppingSystem.Microservice.Settings;

namespace ShoppingSystem.Microservice;

public static class ConfigureService
{
    public static IServiceCollection RegisterDependencies
    (this IServiceCollection services, 
        WebApplicationBuilder builder)
    {
        #region PresentationDependencies

        services.AddAuthentication();
        services.AddAuthorization();

        builder.Services.AddHealthChecks()
            .AddCheck<DbHealthCheck>
            ("ShoppingSystem.Microservice.db")
            .AddCheck<IdentityDbHealthCheck>
            ("ShoppingSystem.Microservice.Identity.db");
        
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter()
                );
            });

        services
        .AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ShoppingSystem.Microservice",
                Version = "v1"
            });

            s.AddSecurityDefinition("Bearer", 
                new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Enter JWT token"
            });

            s.AddSecurityRequirement(document => 
                new OpenApiSecurityRequirement
            {
                [
                    new OpenApiSecuritySchemeReference("Bearer", document)
                ] = []
            });
        });
        
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = false;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;

            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new QueryStringApiVersionReader("api-version"),
                new HeaderApiVersionReader("x.Version"),
                new MediaTypeApiVersionReader("ver"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
        });

        services.AddEndpointsApiExplorer();
        #endregion

        #region LogSettings
        builder.Configuration
            .AddJsonFile(
                "Serilog.json",
                optional: false,
                reloadOnChange: true);
        
        builder.Logging
            .ClearProviders();
        
        builder.Host
            .UseSerilog();
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        
        #endregion
        
        #region RabbitMq
        services.Configure<RabbitMqSetting>(
            builder.Configuration.GetSection("RabbitMq"));
        
        services.AddHostedService<RabbitMqListenerService>();
        
        services.AddScoped<ProductCreatedConsumer>();
        services.AddScoped<ProductDeletedConsumer>();
        services.AddScoped<OrderCreatedConsumer>();

        services.AddScoped
            <IMessagePublisherService, MessagePublisherService>();
        #endregion

        return services;
    }
}