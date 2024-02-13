using FluentAssertions;
using GymLog.Application.Exercises;
using GymLog.Application.Exercises.GetAllExercises;
using GymLog.Domain.Exercises;
using NSubstitute;
using Xunit;

namespace GymLog.Application.Tests.Exercises;

public class GetAllExercisesTests
{
    private readonly GetAllExercisesQueryHandler _handler;

    private readonly IExerciseRepository _exerciseRepository;

    public GetAllExercisesTests()
    {
        _exerciseRepository = Substitute.For<IExerciseRepository>();

        _handler = new GetAllExercisesQueryHandler(_exerciseRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllExercises()
    {
        // Arrange
        _exerciseRepository
            .GetAllWithWorkoutsAsync()
            .Returns(new List<Exercise> { Exercise.Create("Bench Press", ExerciseCategory.Strength) });

        GetAllExercisesQuery query = new();

        // Act
        List<ExerciseDto> exercises = (await _handler.Handle(query, default)).ToList();

        // Assert
        exercises.Should().NotBeNullOrEmpty();
        exercises.Should().HaveCount(1);

        await _exerciseRepository.Received(1).GetAllWithWorkoutsAsync();
    }
}