using FluentAssertions;
using GymLog.Application.Data;
using GymLog.Application.Workouts;
using GymLog.Application.Workouts.DeleteWorkout;
using GymLog.Domain.Workouts;
using GymLog.Domain.Workouts.Exceptions;
using NSubstitute;
using Xunit;

namespace GymLog.Application.Tests.Workouts;

public class DeleteWorkoutTests
{
    private readonly DeleteWorkoutCommandHandler _handler;

    private readonly IWorkoutRepository _workoutRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWorkoutTests()
    {
        _workoutRepository = Substitute.For<IWorkoutRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new DeleteWorkoutCommandHandler(_workoutRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_Should_DeleteExercise_When_ExerciseExists()
    {
        // Arrange
        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 1, 10, Guid.NewGuid());

        _workoutRepository.GetAsync(workout.Id).Returns(workout);

        DeleteWorkoutCommand command = new(workout.Id);

        // Act
        await _handler.Handle(command, default);

        // Assert
        _workoutRepository.Received(1).Delete(Arg.Is<Workout>(x => x.Id == command.Id));

        await _unitOfWork.Received(1).SaveChangesAsync();
    }
    
    [Fact]
    public async Task Handle_Should_ThrowException_When_WorkoutDoesNotExist()
    {
        // Arrange
        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 1, 10, Guid.NewGuid());

        _workoutRepository.GetAsync(workout.Id).Returns(workout);

        DeleteWorkoutCommand command = new(Guid.NewGuid());

        // Act
        Func<Task> result = async () => await _handler.Handle(command, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<WorkoutNotFoundException>()
            .WithMessage("Workout with ID * not found.");
    }
}