using MediatR;

namespace GymLog.Application.Workouts.GetWorkoutDates;

public sealed record GetWorkoutDatesQuery : IRequest<WorkoutDatesDto>;