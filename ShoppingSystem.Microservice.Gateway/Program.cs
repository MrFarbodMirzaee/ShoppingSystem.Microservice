using Microsoft.AspNetCore.Authentication.JwtBearer;
using ShoppingSystem.Microservice.Gateway;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Starting Gateway...");

builder.Configuration.AddJsonFile(
    "Yarp/reverseProxy.json",
    optional: false,
    reloadOnChange: true
);

var proxySection = builder.Configuration.GetSection("ReverseProxy");

Console.WriteLine("=== YARP CONFIG DEBUG ===");
Console.WriteLine($"Section exists: {proxySection.Exists()}");

foreach (var route in proxySection.GetSection("Routes").GetChildren())
{
    Console.WriteLine($"Route: {route.Key}");
}

foreach (var cluster in proxySection.GetSection("Clusters").GetChildren())
{
    Console.WriteLine($"Cluster: {cluster.Key}");
}

Console.WriteLine("=========================");

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(proxySection);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:5001";
        options.RequireHttpsMetadata = false;
        options.Audience = "gateway";
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.RegisterGateWayDependencies(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy(proxyPipeline =>
{
    proxyPipeline.Use(async (context, next) =>
    {
        Console.WriteLine($"Incoming request: {context.Request.Path}");

        context.Request.Headers.TryAdd(
            "X-Correlation-ID",
            Guid.NewGuid().ToString()
        );

        await next();
    });
});

app.Run();