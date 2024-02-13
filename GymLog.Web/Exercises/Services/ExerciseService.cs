using System.Net.Http.Json;
using GymLog.Web.Exercises.Dtos;
using GymLog.Web.Exercises.Requests;
using GymLog.Web.Extensions;

namespace GymLog.Web.Exercises.Services;

public sealed class ExerciseService : IExerciseService
{
    private readonly HttpClient _httpClient;

    public ExerciseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ExerciseDto>> GetExercisesAsync()
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/exercises");

        await httpResponseMessage.EnsureSuccessAsync();

        IEnumerable<ExerciseDto>? exerciseDtos = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ExerciseDto>>();

        return exerciseDtos!;
    }

    public async Task CreateProjectAsync(CreateExerciseRequest request)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync("api/exercises", request);

        await httpResponseMessage.EnsureSuccessAsync();
    }

    public async Task UpdateExerciseAsync(Guid exerciseId, UpdateExerciseRequest request)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync($"api/exercises/{exerciseId}", request);

        await httpResponseMessage.EnsureSuccessAsync();
    }

    public async Task DeleteExerciseAsync(Guid exerciseId)
    {
        HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync($"api/exercises/{exerciseId}");

        await httpResponseMessage.EnsureSuccessAsync();
    }
}