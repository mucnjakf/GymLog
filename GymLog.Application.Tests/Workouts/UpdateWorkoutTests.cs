using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GymLog.Application.Data;
using GymLog.Application.Exercises;
using GymLog.Application.Workouts;
using GymLog.Application.Workouts.UpdateWorkout;
using GymLog.Domain.Exercises;
using GymLog.Domain.Exercises.Exceptions;
using GymLog.Domain.Workouts;
using GymLog.Domain.Workouts.Exceptions;
using NSubstitute;
using Xunit;
using ValidationException = GymLog.Domain.Exceptions.ValidationException;

namespace GymLog.Application.Tests.Workouts;

public class UpdateWorkoutTests
{
    private readonly UpdateWorkoutCommandHandler _handler;

    private readonly IWorkoutRepository _workoutRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateWorkoutCommand> _validator;

    public UpdateWorkoutTests()
    {
        _workoutRepository = Substitute.For<IWorkoutRepository>();
        _exerciseRepository = Substitute.For<IExerciseRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _validator = Substitute.For<IValidator<UpdateWorkoutCommand>>();

        _handler = new UpdateWorkoutCommandHandler(_workoutRepository, _exerciseRepository, _unitOfWork, _validator);
    }

    [Fact]
    public async Task Handle_Should_UpdateWorkout_When_WorkoutExists()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);
        _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 3, 10, exercise.Id);
        _workoutRepository.GetAsync(workout.Id).Returns(workout);

        UpdateWorkoutCommand command = new(workout.Id, "2 hour", DateTime.UtcNow.AddDays(1), 1, 5, exercise.Id);

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult());

        // Act
        await _handler.Handle(command, default);

        // Assert
        await _validator
            .Received(1)
            .ValidateAsync(Arg.Is<UpdateWorkoutCommand>(x =>
                x.Id == command.Id &&
                x.Duration == command.Duration &&
                x.DateTime == command.DateTime &&
                x.Sets == command.Sets &&
                x.Reps == command.Reps &&
                x.ExerciseId == command.ExerciseId));

        await _exerciseRepository.Received(1).GetAsync(Arg.Is(command.ExerciseId));

        await _workoutRepository.Received(1).GetAsync(Arg.Is(command.Id));

        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_ExerciseDoesNotExist()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);
        _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 3, 10, exercise.Id);
        _workoutRepository.GetAsync(workout.Id).Returns(workout);

        UpdateWorkoutCommand command = new(workout.Id, "2 hour", DateTime.UtcNow.AddDays(1), 1, 5, Guid.NewGuid());

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult());

        // Act
        Func<Task> result = async () => await _handler.Handle(command, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<ExerciseNotFoundException>()
            .WithMessage("Exercise with ID * not found.");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_WorkoutDoesNotExist()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);
        _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 3, 10, exercise.Id);
        _workoutRepository.GetAsync(workout.Id).Returns(workout);

        UpdateWorkoutCommand command = new(Guid.NewGuid(), "2 hour", DateTime.UtcNow.AddDays(1), 1, 5, exercise.Id);

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult());

        // Act
        Func<Task> result = async () => await _handler.Handle(command, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<WorkoutNotFoundException>()
            .WithMessage("Workout with ID * not found.");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_CommandIsInvalid()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);
        _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

        Workout workout = Workout.Create("1 hour", DateTime.UtcNow, 3, 10, exercise.Id);
        _workoutRepository.GetAsync(workout.Id).Returns(workout);

        UpdateWorkoutCommand command = new(workout.Id, "", DateTime.UtcNow.AddDays(1), 1, 5, exercise.Id);

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult(new List<ValidationFailure> { new() }));

        // Act
        Func<Task> result = async () => await _handler.Handle(command, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<ValidationException>()
            .WithMessage("UpdateWorkoutCommand is invalid.");
    }
}