using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using ShoppingSystem.Microservice.Infrastructure.Identity.Context;
using ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.Options;

namespace ShoppingSystem.Microservice.Infrastructure.Identity.Persistence.HealthChecks;

public class IdentityDbHealthCheck : IHealthCheck
{
    private readonly IdentityAppDbContext _db;
    private readonly DataBaseOptions _options;

    public IdentityDbHealthCheck(IdentityAppDbContext db
        , IOptions<DataBaseOptions> options)
    {
        _db = db;
        _options = options.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (_db == null)
                return HealthCheckResult
                    .Unhealthy("DbContext is null");

            var canConnect = await _db.Database
                .CanConnectAsync(cancellationToken);

            if (!canConnect)
                return HealthCheckResult.Unhealthy(
                    $"Database connection failed. Provider: SqlService");

            return HealthCheckResult.Healthy(
                $"Database OK (Sql server)");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                $"Database health check failed (SqlService)", ex);
        }
    }
}