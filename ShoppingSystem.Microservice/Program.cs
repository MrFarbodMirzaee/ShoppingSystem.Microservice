using System.Text.Json;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ShoppingSystem.Microservice;
using ShoppingSystem.Microservice.Application;
using ShoppingSystem.Microservice.Infrastructure.Identity;
using ShoppingSystem.Microservice.Middlewares;
using ShoppingSystem.Microservice.Notification.Email;
using ShoppingSystem.Microservice.Notification.Sms;

var builder = WebApplication
    .CreateBuilder(args);

builder.Services
.RegisterDependencies(builder);

builder.Services
    .RegisterApplicationDependencies()
    .RegisterInfrastructureDependencies(builder.Configuration)
    .RegisterIdentityDependencies(builder.Configuration)
    .RegisterEmailDependencies(builder.Configuration)
    .RegisterSmsNotificationDependencies(builder.Configuration);
        
var app = 
    builder.Build();

if (app.Environment
    .IsDevelopment())
{
    app.MapSwagger();
    app.MapSwaggerUI();
}

await app.Services
    .UseIdentitySeederAsync();

await app.Services
    .UseDatabaseSeederAsync();

app.MapHealthChecks("/health", 
    new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                description = x.Value.Description,
                error = x.Value.Exception?.Message
            })
        });

        await context.Response.WriteAsync(result);
    }
});

app.UseMiddleware
    <GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseLogUrl();

app.MapControllers();

app.Run();