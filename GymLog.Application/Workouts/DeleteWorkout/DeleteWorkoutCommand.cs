using MediatR;

namespace GymLog.Application.Workouts.DeleteWorkout;

public sealed record DeleteWorkoutCommand(Guid Id) : IRequest;