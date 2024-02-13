using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GymLog.Application.Data;
using GymLog.Application.Exercises;
using GymLog.Application.Exercises.CreateExercise;
using GymLog.Domain.Exercises;
using NSubstitute;
using Xunit;
using ValidationException = GymLog.Domain.Exceptions.ValidationException;

namespace GymLog.Application.Tests.Exercises;

public class CreateExerciseTests
{
    private readonly CreateExerciseCommandHandler _handler;

    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateExerciseCommand> _validator;

    public CreateExerciseTests()
    {
        _exerciseRepository = Substitute.For<IExerciseRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _validator = Substitute.For<IValidator<CreateExerciseCommand>>();

        _handler = new CreateExerciseCommandHandler(_exerciseRepository, _unitOfWork, _validator);
    }

    [Fact]
    public async Task Handle_Should_CreateExercise_When_CommandIsValid()
    {
        // Arrange
        CreateExerciseCommand command = new("Bench Press", ExerciseCategory.Strength);

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult());

        // Act
        await _handler.Handle(command, default);

        // Assert
        await _validator
            .Received(1)
            .ValidateAsync(Arg.Is<CreateExerciseCommand>(x => x.Name == command.Name && x.Category == command.Category));

        _exerciseRepository
            .Received(1)
            .Insert(Arg.Is<Exercise>(x => x.Name == command.Name && x.Category == command.Category));

        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_CommandIsInvalid()
    {
        // Arrange
        CreateExerciseCommand command = new("", ExerciseCategory.Strength);

        _validator
            .ValidateAsync(command)
            .Returns(new ValidationResult(new List<ValidationFailure> { new() }));

        // Act
        Func<Task> result = async () => await _handler.Handle(command, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<ValidationException>()
            .WithMessage("CreateExerciseCommand is invalid.");
    }
}