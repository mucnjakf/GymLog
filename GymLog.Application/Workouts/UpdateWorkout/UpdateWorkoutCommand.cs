using MediatR;

namespace GymLog.Application.Workouts.UpdateWorkout;

public sealed record UpdateWorkoutCommand(Guid Id, string Duration, DateTime DateTime, int Sets, int Reps, Guid ExerciseId) : IRequest;