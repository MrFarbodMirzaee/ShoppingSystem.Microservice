using Infrastructure.Persistence.Enums;
using Infrastructure.Persistence.Options;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Factories;

public static class OptionFactory
{
    public static Option Create(IConfiguration configuration)
    {
        var section = configuration.GetSection("Database");

        var providerString = section["Provider"];
        var connectionStringName = section["ConnectionStringName"];

        if (string.IsNullOrWhiteSpace(connectionStringName))
            throw new InvalidOperationException("Database:ConnectionStringName is missing.");

        if (string.IsNullOrWhiteSpace(providerString))
            throw new InvalidOperationException("Database:Provider is missing.");

        var connectionString = configuration.GetConnectionString(connectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                $"Connection string '{connectionStringName}' not found.");

        return new Option
        {
            Provider = Enum.Parse<Provider>(providerString),
            ConnectionStringName = connectionStringName,
            ConnectionString = connectionString
        };
    }
}