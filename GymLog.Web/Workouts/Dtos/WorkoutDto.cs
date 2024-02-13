namespace GymLog.Web.Workouts.Dtos;

public sealed record WorkoutDto(Guid Id, string Duration, string DateTime, int Sets, int Reps, WorkoutExerciseDto Exercise);