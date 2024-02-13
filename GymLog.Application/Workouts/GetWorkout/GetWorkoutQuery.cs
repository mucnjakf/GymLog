using MediatR;

namespace GymLog.Application.Workouts.GetWorkout;

public sealed record GetWorkoutQuery(Guid Id) : IRequest<WorkoutDto>;