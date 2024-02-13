using MediatR;

namespace GymLog.Application.Workouts.GetWorkouts;

public sealed record GetWorkoutsQuery(DateTime DateTime) : IRequest<IEnumerable<WorkoutDto>>;