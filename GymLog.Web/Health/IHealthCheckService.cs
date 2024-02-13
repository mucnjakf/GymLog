namespace GymLog.Web.Health;

public interface IHealthCheckService
{
    Task<HealthCheckDto> CheckHealthAsync();
}