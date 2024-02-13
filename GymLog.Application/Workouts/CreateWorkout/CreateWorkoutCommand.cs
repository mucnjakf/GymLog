using MediatR;

namespace GymLog.Application.Workouts.CreateWorkout;

public sealed record CreateWorkoutCommand(string Duration, DateTime DateTime, int Sets, int Reps, Guid ExerciseId) : IRequest<WorkoutDto>;