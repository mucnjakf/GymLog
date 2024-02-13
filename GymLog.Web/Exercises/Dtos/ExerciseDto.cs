namespace GymLog.Web.Exercises.Dtos;

public sealed record ExerciseDto(Guid Id, string Name, string Category, IEnumerable<ExerciseWorkoutDto> Workouts);