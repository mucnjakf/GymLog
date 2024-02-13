using FluentAssertions;
using GymLog.Application.Exercises;
using GymLog.Application.Exercises.GetExercise;
using GymLog.Domain.Exercises;
using GymLog.Domain.Exercises.Exceptions;
using NSubstitute;
using Xunit;

namespace GymLog.Application.Tests.Exercises;

public class GetExerciseTests
{
    private readonly GetExerciseQueryHandler _handler;

    private readonly IExerciseRepository _exerciseRepository;

    public GetExerciseTests()
    {
        _exerciseRepository = Substitute.For<IExerciseRepository>();

        _handler = new GetExerciseQueryHandler(_exerciseRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnExercise_When_ExerciseExists()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);

        _exerciseRepository.GetWithWorkoutsAsync(exercise.Id).Returns(exercise);

        GetExerciseQuery query = new(exercise.Id);

        // Act
        ExerciseDto exerciseDto = await _handler.Handle(query, default);

        // Assert
        exerciseDto.Should().NotBeNull();
        exerciseDto.Id.Should().Be(exercise.Id);
        exerciseDto.Name.Should().Be(exercise.Name);
        exerciseDto.Category.Should().Be(exercise.Category.ToString());
        exerciseDto.Workouts.Should().BeEmpty();

        await _exerciseRepository.Received(1).GetWithWorkoutsAsync(Arg.Is(query.Id));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_ExerciseDoesNotExist()
    {
        // Arrange
        Exercise exercise = Exercise.Create("Bench Press", ExerciseCategory.Strength);

        _exerciseRepository.GetWithWorkoutsAsync(exercise.Id).Returns(exercise);

        GetExerciseQuery query = new(Guid.NewGuid());

        // Act
        Func<Task> result = async () => await _handler.Handle(query, default);

        // Assert
        await result
            .Should()
            .ThrowAsync<ExerciseNotFoundException>()
            .WithMessage($"Exercise with ID * not found.");
    }
}