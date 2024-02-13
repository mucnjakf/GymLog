namespace GymLog.Application.Workouts.CreateWorkout;

public sealed record CreateWorkoutRequest(string Duration, DateTime DateTime, int Sets, int Reps, Guid ExerciseId);