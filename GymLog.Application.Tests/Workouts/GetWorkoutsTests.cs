using FluentAssertions;
using GymLog.Application.Workouts;
using GymLog.Application.Workouts.GetWorkouts;
using GymLog.Domain.Exercises;
using GymLog.Domain.Workouts;
using NSubstitute;
using Xunit;

namespace GymLog.Application.Tests.Workouts;

public class GetWorkoutsTests
{
    private readonly GetWorkoutsQueryHandler _handler;

    private readonly IWorkoutRepository _workoutRepository;

    public GetWorkoutsTests()
    {
        _workoutRepository = Substitute.For<IWorkoutRepository>();

        _handler = new GetWorkoutsQueryHandler(_workoutRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnWorkouts_When_DateIsValid()
    {
        // Arrange
        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 3, 10, Guid.NewGuid());
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);

        workout.AddExercise(exercise);

        GetWorkoutsQuery query = new(workout.DateTime);

        _workoutRepository
            .GetWithExercisesAsync(query.DateTime)
            .Returns(new List<Workout> { workout });

        // Act
        List<WorkoutDto> workoutDtos = (await _handler.Handle(query, default)).ToList();

        // Assert
        workoutDtos.Should().NotBeNullOrEmpty();
        workoutDtos.Should().HaveCount(1);

        await _workoutRepository.Received(1).GetWithExercisesAsync(query.DateTime);
    }
}