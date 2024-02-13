using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GymLog.Application.Data;
using GymLog.Application.Exercises;
using GymLog.Application.Exercises.UpdateExercise;
using GymLog.Domain.Exercises;
using GymLog.Domain.Exercises.Exceptions;
using NSubstitute;
using Xunit;
using ValidationException = GymLog.Domain.Exceptions.ValidationException;

namespace GymLog.Application.Tests.Exercises;

public class UpdateExerciseTests
{
    private readonly UpdateExerciseCommandHandler _handler;

    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateExerciseCommand> _validator;

    public UpdateExerciseTests()
    {
        _exerciseRepository = Substitute.For<IExerciseRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _validator = Substitute.For<IValidator<UpdateExerciseCommand>>();

        _handler = new UpdateExerciseCommandHandler(_exerciseRepository, _unitOfWork, _validator);
    }

    [Fact]
    public async Task Handle_Should_UpdateExercise_When_ExerciseExists()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);

        _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

        UpdateExerciseCommand command = new(exercise.Id, "Bench Press", ExerciseCategory.Strength);

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult());

        // Act
        await _handler.Handle(command, default);

        // Assert
        await _validator
            .Received(1)
            .ValidateAsync(Arg.Is<UpdateExerciseCommand>(x => x.Id == command.Id && x.Name == command.Name && x.Category == command.Category));

        await _exerciseRepository.Received(1).GetAsync(Arg.Is(command.Id));

        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_ExerciseDoesNotExists()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);

        _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

        UpdateExerciseCommand command = new(Guid.NewGuid(), "Bench Press", ExerciseCategory.Strength);

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult());

        // Act
        Func<Task> result = async () => await _handler.Handle(command, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<ExerciseNotFoundException>()
            .WithMessage($"Exercise with ID * not found.");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_CommandIsInvalid()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);

        _exerciseRepository.GetAsync(exercise.Id).Returns(exercise);

        UpdateExerciseCommand command = new(exercise.Id, "", ExerciseCategory.Strength);

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult(new List<ValidationFailure> { new() }));

        // Act
        Func<Task> result = async () => await _handler.Handle(command, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<ValidationException>()
            .WithMessage("UpdateExerciseCommand is invalid.");
    }
}