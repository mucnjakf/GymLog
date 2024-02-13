using FluentAssertions;
using GymLog.Application.Workouts;
using GymLog.Application.Workouts.GetWorkout;
using GymLog.Domain.Exercises;
using GymLog.Domain.Workouts;
using GymLog.Domain.Workouts.Exceptions;
using NSubstitute;
using Xunit;

namespace GymLog.Application.Tests.Workouts;

public class GetWorkoutTests
{
    private readonly GetWorkoutQueryHandler _handler;

    private readonly IWorkoutRepository _workoutRepository;

    public GetWorkoutTests()
    {
        _workoutRepository = Substitute.For<IWorkoutRepository>();

        _handler = new GetWorkoutQueryHandler(_workoutRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnWorkout_When_WorkoutExists()
    {
        // Arrange
        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 3, 10, Guid.NewGuid());
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);

        workout.AddExercise(exercise);

        _workoutRepository.GetWithExerciseAsync(workout.Id).Returns(workout);

        GetWorkoutQuery query = new(workout.Id);

        // Act
        WorkoutDto workoutDto = await _handler.Handle(query, default);

        // Assert
        workoutDto.Should().NotBeNull();
        workoutDto.Id.Should().Be(workout.Id);
        workoutDto.Duration.Should().Be(workout.Duration);
        workoutDto.DateTime.Should().Be(workout.DateTime.ToString("yyyy-MM-dd"));
        workoutDto.Sets.Should().Be(workout.Sets);
        workoutDto.Reps.Should().Be(workout.Reps);
        workoutDto.Exercise.Should().NotBeNull();
        workoutDto.Exercise.Id.Should().Be(exercise.Id);
        workoutDto.Exercise.Name.Should().Be(exercise.Name);
        workoutDto.Exercise.Category.Should().Be(exercise.Category.ToString());

        await _workoutRepository.Received(1).GetWithExerciseAsync(Arg.Is(query.Id));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_WorkoutDoesNotExist()
    {
        // Arrange
        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 3, 10, Guid.NewGuid());

        _workoutRepository.GetWithExerciseAsync(workout.Id).Returns(workout);

        GetWorkoutQuery query = new(Guid.NewGuid());

        // Act
        Func<Task> result = async () => await _handler.Handle(query, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<WorkoutNotFoundException>()
            .WithMessage("Workout with ID * not found.");
    }
}