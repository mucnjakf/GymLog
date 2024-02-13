using System.Net.Http.Json;

namespace GymLog.Web.Health;

public class HealthCheckService : IHealthCheckService
{
    private readonly HttpClient _httpClient;

    public HealthCheckService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HealthCheckDto> CheckHealthAsync()
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/health");

        HealthCheckDto healthCheckDto = (await httpResponseMessage.Content.ReadFromJsonAsync<HealthCheckDto>())!;

        return healthCheckDto;
    }
}