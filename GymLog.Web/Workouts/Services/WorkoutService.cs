using System.Net.Http.Json;
using GymLog.Web.Extensions;
using GymLog.Web.Workouts.Dtos;
using GymLog.Web.Workouts.Requests;

namespace GymLog.Web.Workouts.Services;

public sealed class WorkoutService : IWorkoutService
{
    private readonly HttpClient _httpClient;

    public WorkoutService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<WorkoutDto>> GetWorkoutsAsync()
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/workouts");

        await httpResponseMessage.EnsureSuccessAsync();

        IEnumerable<WorkoutDto>? workoutDtos = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<WorkoutDto>>();

        return workoutDtos!;
    }

    public async Task<IEnumerable<WorkoutDto>> GetWorkoutsAsync(DateTime dateTime)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"api/workouts/{dateTime:yyyy-MM-dd}");

        await httpResponseMessage.EnsureSuccessAsync();

        IEnumerable<WorkoutDto>? workoutDtos = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<WorkoutDto>>();

        return workoutDtos!;
    }

    public async Task<WorkoutDatesDto> GetWorkoutDatesAsync()
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/workouts/dates");

        await httpResponseMessage.EnsureSuccessAsync();

        WorkoutDatesDto? workoutDatesDto = await httpResponseMessage.Content.ReadFromJsonAsync<WorkoutDatesDto>();

        return workoutDatesDto!;
    }

    public async Task CreateWorkoutAsync(CreateWorkoutRequest request)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync("api/workouts", request);

        await httpResponseMessage.EnsureSuccessAsync();
    }

    public async Task UpdateWorkoutAsync(Guid workoutId, UpdateWorkoutRequest request)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync($"api/workouts/{workoutId}", request);

        await httpResponseMessage.EnsureSuccessAsync();
    }

    public async Task DeleteWorkoutAsync(Guid workoutId)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync($"api/workouts/{workoutId}");

        await httpResponseMessage.EnsureSuccessAsync();
    }
}