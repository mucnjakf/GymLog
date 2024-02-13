using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GymLog.Application.Exercises;
using GymLog.Application.Exercises.CreateExercise;
using GymLog.Application.Exercises.UpdateExercise;
using GymLog.Application.Workouts.CreateWorkout;
using GymLog.Domain.Exercises;
using Xunit;

namespace GymLab.IntegrationTests.Exercises;

public class ExercisesEndpointsTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public ExercisesEndpointsTests(TestingWebAppFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllExercises_Should_ReturnExercises()
    {
        // Arrange
        await SeedExerciseAsync();

        // Act
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync("api/exercises");
        httpResponseMessage.EnsureSuccessStatusCode();

        IEnumerable<ExerciseDto> exercises = (await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ExerciseDto>>())!.ToList();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        exercises.Should().NotBeNullOrEmpty();
        exercises.Should().HaveCountGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task GetExercises_Should_ReturnExercise_When_ExerciseFound()
    {
        // Arrange
        ExerciseDto exerciseFromDb = await SeedExerciseAsync();

        // Act
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"api/exercises/{exerciseFromDb.Id}");
        httpResponseMessage.EnsureSuccessStatusCode();

        ExerciseDto exercise = (await httpResponseMessage.Content.ReadFromJsonAsync<ExerciseDto>())!;

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        exercise.Should().NotBeNull();
        exercise.Id.Should().Be(exerciseFromDb.Id);
        exercise.Name.Should().Be(exerciseFromDb.Name);
        exercise.Category.Should().Be(exerciseFromDb.Category);
    }

    [Fact]
    public async Task GetExercises_Should_ReturnNotFound_When_ExerciseNotFound()
    {
        // Arrange
        await SeedExerciseAsync();

        // Act
        HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"api/exercises/{Guid.NewGuid()}");

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateExercise_Should_ReturnCreated()
    {
        // Arrange
        CreateExerciseRequest request = new("Exercise", ExerciseCategory.None);

        // Act
        HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync("api/exercises", request);
        httpResponseMessage.EnsureSuccessStatusCode();

        ExerciseDto exercise = (await httpResponseMessage.Content.ReadFromJsonAsync<ExerciseDto>())!;

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
        exercise.Should().NotBeNull();
        exercise.Id.Should().NotBeEmpty();
        exercise.Name.Should().Be(request.Name);
        exercise.Category.Should().Be(request.Category.ToString());
    }

    [Fact]
    public async Task CreateExercise_Should_ReturnBadRequest_When_ValidationFails()
    {
        // Arrange
        CreateExerciseRequest request = new(string.Empty, ExerciseCategory.None);

        // Act
        HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync("api/exercises", request);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateExercise_Should_ReturnNoContent()
    {
        // Arrange
        ExerciseDto exerciseFromDb = await SeedExerciseAsync();

        UpdateExerciseRequest request = new("Bench press", ExerciseCategory.Strength);

        // Act
        HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync($"api/exercises/{exerciseFromDb.Id}", request);
        httpResponseMessage.EnsureSuccessStatusCode();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateExercise_Should_ReturnBadRequest_When_ValidationFails()
    {
        // Arrange
        ExerciseDto exerciseFromDb = await SeedExerciseAsync();

        UpdateExerciseRequest request = new(string.Empty, ExerciseCategory.Strength);

        // Act
        HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync($"api/exercises/{exerciseFromDb.Id}", request);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UpdateExercise_Should_ReturnNotFound_When_ExerciseNotFound()
    {
        // Arrange
        await SeedExerciseAsync();

        UpdateExerciseRequest request = new("Bench press", ExerciseCategory.Strength);

        // Act
        HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync($"api/exercises/{Guid.NewGuid()}", request);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteExercise_Should_ReturnNoContent()
    {
        // Arrange
        ExerciseDto exerciseFromDb = await SeedExerciseAsync();

        // Act
        HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync($"api/exercises/{exerciseFromDb.Id}");
        httpResponseMessage.EnsureSuccessStatusCode();

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteExercise_Should_ReturnNotFound_When_ExerciseNotFound()
    {
        // Arrange
        await SeedExerciseAsync();

        // Act
        HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync($"api/exercises/{Guid.NewGuid()}");

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteExercise_Should_ReturnConflict_When_ExerciseContainsWorkouts()
    {
        // Arrange
        ExerciseDto exerciseFromDb = await SeedExerciseWithWorkoutAsync();

        // Act
        HttpResponseMessage httpResponseMessage = await _httpClient.DeleteAsync($"api/exercises/{exerciseFromDb.Id}");

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    private async Task<ExerciseDto> SeedExerciseAsync()
    {
        HttpResponseMessage message = await _httpClient
            .PostAsJsonAsync("api/exercises", new CreateExerciseRequest($"Exercise - {Guid.NewGuid()}", ExerciseCategory.None));

        return (await message.Content.ReadFromJsonAsync<ExerciseDto>())!;
    }

    private async Task<ExerciseDto> SeedExerciseWithWorkoutAsync()
    {
        HttpResponseMessage message = await _httpClient
            .PostAsJsonAsync("api/exercises", new CreateExerciseRequest($"Exercise - {Guid.NewGuid()}", ExerciseCategory.None));

        ExerciseDto exercise = (await message.Content.ReadFromJsonAsync<ExerciseDto>())!;

        await _httpClient
            .PostAsJsonAsync("api/workouts", new CreateWorkoutRequest("1 hour", DateTime.Now, 3, 10, exercise.Id));

        return exercise;
    }
}