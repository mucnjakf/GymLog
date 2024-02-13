using FluentAssertions;
using GymLog.Application.Workouts;
using GymLog.Application.Workouts.GetAllWorkouts;
using GymLog.Domain.Exercises;
using GymLog.Domain.Workouts;
using NSubstitute;
using Xunit;

namespace GymLog.Application.Tests.Workouts;

public class GetAllWorkoutsTests
{
    private readonly GetAllWorkoutsQueryHandler _handler;

    private readonly IWorkoutRepository _workoutRepository;

    public GetAllWorkoutsTests()
    {
        _workoutRepository = Substitute.For<IWorkoutRepository>();

        _handler = new GetAllWorkoutsQueryHandler(_workoutRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllWorkouts()
    {
        // Arrange
        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 3, 10, Guid.NewGuid());
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);

        workout.AddExercise(exercise);

        _workoutRepository
            .GetAllWithExerciseAsync()
            .Returns(new List<Workout> { workout });

        GetAllWorkoutsQuery query = new();

        // Act
        List<WorkoutDto> workouts = (await _handler.Handle(query, default)).ToList();

        // Assert
        workouts.Should().NotBeNullOrEmpty();
        workouts.Should().HaveCount(1);

        await _workoutRepository.Received(1).GetAllWithExerciseAsync();
    }
}