namespace GymLog.Web.Exercises.Dtos;

public sealed record ExerciseWorkoutDto(Guid Id, string Duration, string DateTime, int Sets, int Reps);