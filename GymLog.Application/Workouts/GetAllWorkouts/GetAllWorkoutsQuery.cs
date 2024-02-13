using MediatR;

namespace GymLog.Application.Workouts.GetAllWorkouts;

public sealed record GetAllWorkoutsQuery : IRequest<IEnumerable<WorkoutDto>>;