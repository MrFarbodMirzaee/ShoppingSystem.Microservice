using Infrastructure.Contexts;
using Infrastructure.Persistence.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.HealthChecks;

public class DbHealthCheck : IHealthCheck
{
    private readonly AppDbContext _db;
    private readonly Option _options;

    public DbHealthCheck(AppDbContext db,
        IOptions<Option> options)
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
                return HealthCheckResult.Unhealthy("DbContext is null");

            var canConnect = await _db.Database
                .CanConnectAsync(cancellationToken);

            if (!canConnect)
                return HealthCheckResult.Unhealthy(
                    $"Database connection failed. Provider: {_options.Provider}");

            return HealthCheckResult.Healthy(
                $"Database OK ({_options.Provider})");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                $"Database health check failed ({_options.Provider})", ex);
        }
    }
}