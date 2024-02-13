using FluentAssertions;
using GymLog.Application.Data;
using GymLog.Application.Exercises;
using GymLog.Application.Exercises.DeleteExercise;
using GymLog.Domain.Exercises;
using GymLog.Domain.Exercises.Exceptions;
using GymLog.Domain.Workouts;
using NSubstitute;
using Xunit;

namespace GymLog.Application.Tests.Exercises;

public class DeleteExerciseTests
{
    private readonly DeleteExerciseCommandHandler _handler;

    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExerciseTests()
    {
        _exerciseRepository = Substitute.For<IExerciseRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new DeleteExerciseCommandHandler(_exerciseRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_DeleteExercise_When_ExerciseExists()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);

        _exerciseRepository.GetWithWorkoutsAsync(exercise.Id).Returns(exercise);

        DeleteExerciseCommand command = new(exercise.Id);

        // Act
        await _handler.Handle(command, default);

        // Assert
        _exerciseRepository.Received(1).Delete(Arg.Is<Exercise>(x => x.Id == command.Id));

        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_ExerciseDoesNotExist()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);

        _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

        DeleteExerciseCommand command = new(Guid.NewGuid());

        // Act
        Func<Task> result = async () => await _handler.Handle(command, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<ExerciseNotFoundException>()
            .WithMessage($"Exercise with ID * not found.");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_ContainsWorkouts()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);
        exercise.AddWorkout(Workout.Create("1 hour", DateTime.UtcNow, 3, 10, exercise.Id));

        _exerciseRepository.GetWithWorkoutsAsync(exercise.Id).Returns(exercise);

        DeleteExerciseCommand command = new(exercise.Id);

        // Act
        Func<Task> result = async () => await _handler.Handle(command, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<ExerciseContainsWorkoutsException>()
            .WithMessage("Unable to delete exercise that contains workouts.");
    }
}