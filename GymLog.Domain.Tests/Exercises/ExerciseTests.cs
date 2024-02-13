using FluentAssertions;
using GymLog.Domain.Exercises;
using GymLog.Domain.Workouts;
using Xunit;

namespace GymLog.Domain.Tests.Exercises;

public class ExerciseTests
{
    [Fact]
    public void Create_Should_ReturnUser_When_ParametersAreValid()
    {
        // Arrange
        const string name = "Bench Press";
        const ExerciseCategory category = ExerciseCategory.Strength;

        // Act
        Exercise exercise = Exercise.Create(name, category);

        // Assert
        exercise.Should().NotBeNull();
        exercise.Name.Should().Be(name);
        exercise.Category.Should().Be(category);
    }

    [Fact]
    public void Update_Should_UpdateExercise_When_ParametersAreValid()
    {
        // Arrange
        const string name = "Bench Press";
        const ExerciseCategory category = ExerciseCategory.Strength;

        Exercise exercise = Exercise.Create(name, category);

        const string newName = "Running";
        const ExerciseCategory newCategory = ExerciseCategory.Cardio;

        // Act
        exercise.Update(newName, newCategory);

        // Assert
        exercise.Should().NotBeNull();
        exercise.Name.Should().Be(newName);
        exercise.Category.Should().Be(newCategory);
    }

    [Fact]
    public void AddWorkout_ShouldAddWorkout_When_ParameterIsValid()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);
        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 3, 10, exercise.Id);

        // Act
        exercise.AddWorkout(workout);

        // Assert
        workout.Should().NotBeNull();
        exercise.Workouts.Should().NotBeNull();
        exercise.Workouts.Should().HaveCount(1);
        exercise.Workouts?.First().Should().Be(workout);
    }
}