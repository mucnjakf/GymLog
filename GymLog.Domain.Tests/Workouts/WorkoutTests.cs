using FluentAssertions;
using GymLog.Domain.Exercises;
using GymLog.Domain.Workouts;
using Xunit;

namespace GymLog.Domain.Tests.Workouts;

public class WorkoutTests
{
    [Fact]
    public void Create_Should_ReturnWorkout_When_ParametersAreValid()
    {
        // Arrange
        const string duration = "1 hour";
        DateTime dateTime = DateTime.UtcNow;
        const int sets = 3;
        const int reps = 10;
        Guid exerciseId = Guid.NewGuid();

        // Act
        Workout workout = Workout.Create(duration, dateTime, sets, reps, exerciseId);

        // Assert
        workout.Should().NotBeNull();
        workout.Duration.Should().Be(duration);
        workout.DateTime.Should().Be(dateTime);
        workout.Sets.Should().Be(sets);
        workout.Reps.Should().Be(reps);
        workout.ExerciseId.Should().Be(exerciseId);
    }

    [Fact]
    public void Update_Should_UpdateWorkout_When_ParametersAreValid()
    {
        // Arrange
        const string duration = "1 hour";
        DateTime dateTime = DateTime.UtcNow;
        const int sets = 3;
        const int reps = 10;
        Guid exerciseId = Guid.NewGuid();

        Workout workout = Workout.Create(duration, dateTime, sets, reps, exerciseId);

        const string newDuration = "2 hours";
        DateTime newDateTime = DateTime.UtcNow.AddDays(1);
        const int newSets = 1;
        const int newReps = 5;
        Guid newExerciseId = Guid.NewGuid();

        // Act
        workout.Update(newDuration, newDateTime, newSets, newReps, newExerciseId);

        // Assert
        workout.Should().NotBeNull();
        workout.Duration.Should().Be(newDuration);
        workout.DateTime.Should().Be(newDateTime);
        workout.Sets.Should().Be(newSets);
        workout.Reps.Should().Be(newReps);
        workout.ExerciseId.Should().Be(newExerciseId);
    }

    [Fact]
    public void AddExercise_Should_AddExercise_WhenParameterIsValid()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);
        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 3, 10, exercise.Id);

        // Act
        workout.AddExercise(exercise);

        // Assert
        workout.Should().NotBeNull();
        workout.Exercise.Should().NotBeNull();
        workout.Exercise.Should().Be(exercise);
    }
}