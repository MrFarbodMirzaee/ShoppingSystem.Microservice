using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ShoppingSystem.Microservice.Infrastructure.Identity.Context;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Options;

namespace ShoppingSystem.Microservice.Infrastructure.Identity.Factories;

public class IdentityAppDbContextFactory 
    : IDesignTimeDbContextFactory<IdentityAppDbContext>
{
    public IdentityAppDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory()
                , "..\\ShoppingSystem.Microservice"));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddUserSecrets<IdentityAppDbContext>(optional: true)
            .AddEnvironmentVariables()
            .Build();

        var dbOptions = configuration
            .GetSection("Database")
            .Get<DataBaseOptions>();

        if (dbOptions == null)
            throw new Exception("DatabaseOptions is missing in configuration");

        var connectionString = configuration.GetConnectionString(
            dbOptions.IdentityConnectionStringName);

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Identity connection string is missing");

        var optionsBuilder = new 
            DbContextOptionsBuilder<IdentityAppDbContext>();

        optionsBuilder.UseSqlServer(connectionString);

        return new IdentityAppDbContext(optionsBuilder.Options);
    }
}