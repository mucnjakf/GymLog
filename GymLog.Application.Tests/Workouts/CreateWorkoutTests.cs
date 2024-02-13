using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GymLog.Application.Data;
using GymLog.Application.Exercises;
using GymLog.Application.Workouts;
using GymLog.Application.Workouts.CreateWorkout;
using GymLog.Domain.Exercises;
using GymLog.Domain.Exercises.Exceptions;
using GymLog.Domain.Workouts;
using NSubstitute;
using Xunit;
using ValidationException = GymLog.Domain.Exceptions.ValidationException;

namespace GymLog.Application.Tests.Workouts;

public class CreateWorkoutTests
{
    private readonly CreateWorkoutCommandHandler _handler;

    private readonly IWorkoutRepository _workoutRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateWorkoutCommand> _validator;

    public CreateWorkoutTests()
    {
        _workoutRepository = Substitute.For<IWorkoutRepository>();
        _exerciseRepository = Substitute.For<IExerciseRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _validator = Substitute.For<IValidator<CreateWorkoutCommand>>();

        _handler = new CreateWorkoutCommandHandler(_workoutRepository, _exerciseRepository, _unitOfWork, _validator);
    }

    [Fact]
    public async Task Handle_Should_CreateWorkout_When_CommandIsValid()
    {
        // Arrange
        Guid exerciseId = Guid.NewGuid();

        _exerciseRepository
            .GetAsync(exerciseId)
            .Returns(Exercise.Create("Bench press", ExerciseCategory.Strength));

        CreateWorkoutCommand command = new("1 hour", DateTime.UtcNow, 3, 10, exerciseId);

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult());

        // Act
        await _handler.Handle(command, default);

        // Assert
        await _validator
            .Received(1)
            .ValidateAsync(Arg.Is<CreateWorkoutCommand>(x =>
                x.Duration == command.Duration &&
                x.DateTime == command.DateTime &&
                x.Sets == command.Sets &&
                x.Reps == command.Reps &&
                x.ExerciseId == command.ExerciseId));

        await _exerciseRepository.Received(1).GetAsync(exerciseId);

        _workoutRepository
            .Received(1)
            .Insert(Arg.Is<Workout>(x =>
                x.Duration == command.Duration &&
                x.DateTime == command.DateTime &&
                x.Sets == command.Sets &&
                x.Reps == command.Reps));

        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_ExerciseDoesNotExist()
    {
        // Arrange
        Guid exerciseId = Guid.NewGuid();

        _exerciseRepository
            .GetAsync(exerciseId)
            .Returns(Exercise.Create("Bench press", ExerciseCategory.Strength));

        CreateWorkoutCommand command = new("1 hour", DateTime.UtcNow, 3, 10, Guid.NewGuid());

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
    public async Task Handle_Should_ThrowException_When_CommandIsInvalid()
    {
        // Arrange
        Guid exerciseId = Guid.NewGuid();

        _exerciseRepository
            .GetAsync(exerciseId)
            .Returns(Exercise.Create("Bench press", ExerciseCategory.Strength));

        CreateWorkoutCommand command = new("", DateTime.UtcNow, 3, 10, exerciseId);

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult(new List<ValidationFailure> { new() }));

        // Act
        Func<Task> result = async () => await _handler.Handle(command, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<ValidationException>()
            .WithMessage("CreateWorkoutCommand is invalid.");
    }
}