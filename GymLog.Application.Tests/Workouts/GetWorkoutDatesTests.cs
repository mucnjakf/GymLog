using FluentAssertions;
using GymLog.Application.Workouts;
using GymLog.Application.Workouts.GetWorkoutDates;
using NSubstitute;
using Xunit;

namespace GymLog.Application.Tests.Workouts;

public class GetWorkoutDatesTests
{
    private readonly GetWorkoutDatesQueryHandler _handler;

    private readonly IWorkoutRepository _workoutRepository;

    public GetWorkoutDatesTests()
    {
        _workoutRepository = Substitute.For<IWorkoutRepository>();

        _handler = new GetWorkoutDatesQueryHandler(_workoutRepository);
    }

    [Fact]
    public async Task Handle_Should_ReturnWorkoutDates()
    {
        // Arrange
        GetWorkoutDatesQuery query = new();

        _workoutRepository
            .GetDatesAsync()
            .Returns(new List<DateTime> { DateTime.Now });

        // Act
        WorkoutDatesDto workoutDatesDto = await _handler.Handle(query, default);

        // Assert
        workoutDatesDto.Should().NotBeNull();
        workoutDatesDto.DateTimes.Should().NotBeEmpty();
        workoutDatesDto.DateTimes.Should().HaveCount(1);
    }
}