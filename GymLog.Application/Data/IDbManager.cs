namespace GymLog.Application.Data;

public interface IDbManager
{
    Task ExecuteHealthCheckAsync();
}