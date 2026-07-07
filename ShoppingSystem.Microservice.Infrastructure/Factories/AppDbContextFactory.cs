using Infrastructure.Contexts;
using Infrastructure.Persistence.Contracts.Base;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Factories;

public class AppDbContextFactory
    : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "../ShoppingSystem.Microservice"
        );
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{Environment
            .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddUserSecrets<AppDbContextFactory>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var option = OptionFactory.Create(configuration);
        
        Console.WriteLine($"Provider: {option.Provider}");
        Console.WriteLine($"Connection: {option.ConnectionString}");

        return new AppDbContext(
            UnitOfWorkBase.CreateOptions(option),option);
    }
}