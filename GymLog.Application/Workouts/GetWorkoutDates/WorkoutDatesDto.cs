namespace GymLog.Application.Workouts.GetWorkoutDates;

public sealed record WorkoutDatesDto(IEnumerable<DateTime> DateTimes);