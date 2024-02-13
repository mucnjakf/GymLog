using GymLog.Application.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GymLog.Api.Health;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbManager _dbManager;

    public DatabaseHealthCheck(IDbManager dbManager)
    {
        _dbManager = dbManager;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        try
        {
            await _dbManager.ExecuteHealthCheckAsync();

            return HealthCheckResult.Healthy();
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy(e.Message);
        }
    }
}