using MediatR;

namespace GymLog.Application.Workouts.UpdateWorkout;

public sealed record UpdateWorkoutRequest(string Duration, DateTime DateTime, int Sets, int Reps, Guid ExerciseId) : IRequest;